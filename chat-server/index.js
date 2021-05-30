const express = require('express');
const app = express();
const http = require('http').Server(app);
const io = require('socket.io')(http);
const redisAdapter = require('socket.io-redis')('redis://127.0.0.1:6379');
const Redis = require('ioredis');

const host = '127.0.0.1';
const port = 6000;
const redis = new Redis(6379, host);

const room_global = 'room-global';

http.listen(port, function () {
    console.log('server on!');
});

// 유저가 연결 event를 Emit하면
io.on('connect', function (socket) {
    console.log('user connected: ' + socket.id);

    socket.emit('connected');

    // 유저가 들어오면 room-global로 입장
    socket.join(room_global, function (err) {
        redis.lrange(room_global, 0, 50)
            .then(function (prevChatList) {
                var historyList = [];

                // 제일 오래된게 제일 앞으로 오게 Reversing
                for (var i = prevChatList.length - 1; i >= 0; --i) {
                    var parsedData = JSON.parse(prevChatList[i]);
                    historyList.push(parsedData);
                }

                io.to(socket.id).emit('receive history', JSON.stringify(historyList));
            })
            .catch(function (err) {
                console.error(err);
            });
    });

    // 유저가 클라에서 해제하면 들어오는 Event
    socket.on('disconnect', function () {
        console.log('user disconnected: ', socket.id);
        socket.leave(room_global);
    });

    // [Client -> Server] ChatData를 보냄
    socket.on('chat', function (data) {
        var parsedData = JSON.parse(data);

        if (parsedData.chatType === 0) // global
        {
            io.to(room_global).emit('receive message', parsedData);

            Lpush(room_global, data)
                .then(Ltrim(room_global))
                .catch(function (err) { console.log(err) });
        }
    });
});

var Lpush = function (channel, data) {
    return redis.lpush(channel, data);
}

var Ltrim = function (channel) {
    return redis.ltrim(channel, 0, 50);
}

var pubClient = redisAdapter.pubClient;
var subClient = redisAdapter.subClient;

pubClient.on('error', function (err) { console.log('err: ', err) });
subClient.on('error', function (err) { console.log('err: ', err) });

io.adapter(redisAdapter);