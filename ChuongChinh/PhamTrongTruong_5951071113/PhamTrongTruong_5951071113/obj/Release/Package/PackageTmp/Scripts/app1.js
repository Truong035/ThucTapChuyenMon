//var apiKey = "AIzaSyAe1AZIJt4jBHfIXax24gRnHkMjbtDNTcs";
//var apiKey=  "AIzaSyDhZSYN5EwyGGca3gowKcuTVtbRkRG4jNE";
//var apiKey = "AIzaSyAvfQsfzyQxamlQGV8FMoCVNDDBvN21ytI";
//var apiKey ="AIzaSyD_m3dXgu6v3shueP5hhJ96goFfK_IBLMw"
//  var apiKey = "AIzaSyDl47TLwMV9xNBcGnJ80v4Q_HRvXYhawQk";
var apiKey = "AIzaSyBVSUFitg7i2VqxZOy9L5 - IVeYR9CCK2Kg"
//var apiKey = "AIzaSyAn-8pHlSteiYY3WlIbau9DZTLQMVV34dI";
var arrQueue = [];
var isPlay = false;
var player;
var countStateUnstarted = 0;
var doubleNext = false;
var data1 = [];

function init() {
    gapi.client.setApiKey(apiKey);
    gapi.client.load("youtube", "v3", function () {
        console.log('Youtube API already');
    });
}

// event enter input tag

function search(keyword) {
   
        $.get(
            "https://www.googleapis.com/youtube/v3/search", {
            part: 'snippet,id',
            q: keyword,
            maxResults: 100,
            type: 'video',
            key: apiKey
        },
           
                    function (data) {
            // clear list search
    

            // console.log("test: " + JSON.stringify(data));
                        $('#frmright').empty();

                       

            var dem = 0;
                        // alert(data.items.length);
                        var sl =0 ;          data1 = data;
            for (var i = 0; i < data.items.length; i++) {
                var content = "";
                if (data.items[i].snippet.channelId == 'UC7Ndf6Vo7E8ut3DP_4CISCQ') {
                    if (dem == 0) {
                        addVideoToPlay(data.items[i]);
                        dem++;
                    }
                    if (sl < 3) {
                        content = content + getResults(data.items[i], i);
                        $('#frmright').append(content);
                        sl++;
                    }       
                }
                        }
                        if (dem == 0 || sl<3) {
                            for (var i = 0; i < data.items.length; i++) {;
                                var content = "";
                            //    if (data.items[i].snippet.channelId == 'UC7Ndf6Vo7E8ut3DP_4CISCQ') {
                                    if (dem == 0) {
                                        addVideoToPlay(data.items[i]);
                                        dem++;
                                    }
                                    if (sl < 3) {
                                        content = content + getResults(data.items[i], i);
                                        $('#frmright').append(content);
                                        sl++;
                                    }


                               }
                        }
                       
                        $('#video').slideDown();
        },
         )       
                //$('#load').hide();
                //// show list video search
                //$('#result-search').append(content);


}


// return item search
function getResults(item, i) {
    // get properties of item
    var title = item.snippet.title;
    var kenh = item.snippet.channelId;
    var thumb = item.snippet.thumbnails.medium.url;
    var channelTitle = item.snippet.channelTitle;
    // onclick = 'phat(id)'id = '" + i + "' > "
    var output = `<img src="` + thumb + `" class="rounded" alt="Responsive image" onclick = 'phat(id)' id = '`+ i + `'/>
        <h4 class="col-12 text-truncate">`+ title + `</h4> ` ;
    return output;

}
function phat(i) {

    addVideoToPlay(data1.items[i]);
    var scrollTrigger = 100, // px
        backToTop = function () {
            var scrollTop = $(window).scrollTop();
            if (scrollTop > scrollTrigger) {
                $('#back-to-top').addClass('show');
            } else {
                $('#back-to-top').removeClass('show');
            }
        };
    // backToTop();
    $(window).on('scroll', function () {
        backToTop();
    });


    $('html,body').animate({
        scrollTop: 0
    }, 10);

}
function addVideoToPlay(item) {

    var title = item.snippet.title;
    var kenh = item.snippet.channelId;
    var thumb = item.snippet.thumbnails.high.url;
    var channelTitle = item.snippet.channelTitle;
    var videoID = item.id.videoId;
    var video = `<div class="embed-responsive embed-responsive-16by9">`
    video += '<iframe class="embed-responsive-item" src="https://www.youtube.com/embed/' + videoID + '?rel=0" allowfullscreen></iframe>';
    video+=   `</div>
        <h3 class="card-title text-info">`+ title+`</h3>`
    $('#frmleft').empty();

    $('#frmleft').append(video);


}



