$(document).ready(function () {
    var player = videojs("my-video");
    $("#btnStopVideo").click(function () {
        player.pause().currentTime(0).trigger('loadstart');
    })
    player.ControlBar = {
        children: {
            'volumeMenuButton': {
                'volumeBar': {
                    'vertical': true
                }
            }
        }
    };
});