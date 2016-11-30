var my = my || {}; //Namespace


$(document).ready(function () {
    my.rootUrl = $("#linkRootUrl").attr("href");
    if (my.rootUrl == "/") {
        my.rootUrl = "";
    }
    
    my.xhrRequestcount = 0;
    
    my.toggleLoader = function (load)
    {
        if (typeof (load) == 'undefined' || !load)
        {
            my.xhrRequestcount--;
           // console.log(my.xhrRequestcount);
            setTimeout(function ()
            {
                if (my.xhrRequestcount <= 0)
                {
                    
                    $('div#loaderWrapper').fadeOut('slow');
                    my.xhrRequestcount = 0;
                }
            }, 100);
        }
        else
        {
            my.xhrRequestcount++;
         //   console.log(my.xhrRequestcount);
            $('div#loaderWrapper').fadeIn('slow');
        }
    };
    
    my.calculateLastMonday = function ()
    {
        var addendum = 0;
        switch (moment().day())
        {
            case 0:
                addendum = -6;
                break;
            case 1:
                addendum = -7;
                break;
            case 2:
                addendum = -8;
                break;
            case 3:
                addendum = -9;
                break;
            case 4:
                addendum = -10;
                break;

            case 6:
                addendum = -5;
                break;
            default:
                addendum = -4;
                break;
        }
        return moment().add(addendum, 'days').format('MM/DD/YYYY');
    },
            my.calculateLastFriday = function ()
            {
                var addendum = 0;
                switch (moment().day())
                {
                    case 0:
                        addendum = -2;
                        break;
                    case 1:
                        addendum = -3;
                        break;
                    case 2:
                        addendum = -4;
                        break;
                    case 3:
                        addendum = -5;
                        break;
                    case 4:
                        addendum = -6;
                        break;

                    case 6:
                        addendum = -1;
                        break;
                    default:
                        addendum = 0;
                        break;
                }
                return moment().add(addendum, 'days').format('MM/DD/YYYY');
            },
   

    my.queryParams = (function (a) {
        if (a == "") return {};
        var b = {};
        for (var i = 0; i < a.length; ++i) {
            var p = a[i].split('=', 2);
            if (p.length == 1)
                b[p[0]] = "";
            else
                b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
        }
        return b;
    })(window.location.search.substr(1).split('&'));
    
    my.isNullorEmpty = function(value) {
        return typeof (value) === 'undefined' || value === null || value === '' || ((typeof (value) === 'string') && (value.trim() === ''));
    };
    my.reset = function (obj) {
        for (var prop in obj) {
            if (obj.hasOwnProperty(prop) && ko.isObservable(obj[prop])) {
                obj[prop]('');
            }
        }
    };

    my.validateEmail = function(email) {
        var regExEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return regExEmail.test(email);
    };
    
    my.consoleMessage = "Training Tracker";
    my.consoleCss = "text-shadow: -1px -1px hsl(0,100%,50%), 1px 1px hsl(5.4, 100%, 50%), 3px 2px hsl(10.8, 100%, 50%), 5px 3px hsl(16.2, 100%, 50%), 7px 4px hsl(21.6, 100%, 50%), 9px 5px hsl(27, 100%, 50%), 11px 6px hsl(32.4, 100%, 50%), 13px 7px hsl(37.8, 100%, 50%), 14px 8px hsl(43.2, 100%, 50%), 16px 9px hsl(48.6, 100%, 50%), 18px 10px hsl(54, 100%, 50%), 20px 11px hsl(59.4, 100%, 50%), 22px 12px hsl(64.8, 100%, 50%), 23px 13px hsl(70.2, 100%, 50%), 25px 14px hsl(75.6, 100%, 50%), 27px 15px hsl(81, 100%, 50%), 28px 16px hsl(86.4, 100%, 50%), 30px 17px hsl(91.8, 100%, 50%), 32px 18px hsl(97.2, 100%, 50%), 33px 19px hsl(102.6, 100%, 50%), 35px 20px hsl(108, 100%, 50%), 36px 21px hsl(113.4, 100%, 50%), 38px 22px hsl(118.8, 100%, 50%), 39px 23px hsl(124.2, 100%, 50%), 41px 24px hsl(129.6, 100%, 50%), 42px 25px hsl(135, 100%, 50%), 43px 26px hsl(140.4, 100%, 50%), 45px 27px hsl(145.8, 100%, 50%), 46px 28px hsl(151.2, 100%, 50%), 47px 29px hsl(156.6, 100%, 50%), 48px 30px hsl(162, 100%, 50%), 49px 31px hsl(167.4, 100%, 50%), 50px 32px hsl(172.8, 100%, 50%), 51px 33px hsl(178.2, 100%, 50%), 52px 34px hsl(183.6, 100%, 50%), 53px 35px hsl(189, 100%, 50%), 54px 36px hsl(194.4, 100%, 50%), 55px 37px hsl(199.8, 100%, 50%), 55px 38px hsl(205.2, 100%, 50%), 56px 39px hsl(210.6, 100%, 50%), 57px 40px hsl(216, 100%, 50%), 57px 41px hsl(221.4, 100%, 50%), 58px 42px hsl(226.8, 100%, 50%), 58px 43px hsl(232.2, 100%, 50%), 58px 44px hsl(237.6, 100%, 50%), 59px 45px hsl(243, 100%, 50%), 59px 46px hsl(248.4, 100%, 50%), 59px 47px hsl(253.8, 100%, 50%), 59px 48px hsl(259.2, 100%, 50%), 59px 49px hsl(264.6, 100%, 50%), 60px 50px hsl(270, 100%, 50%), 59px 51px hsl(275.4, 100%, 50%), 59px 52px hsl(280.8, 100%, 50%), 59px 53px hsl(286.2, 100%, 50%), 59px 54px hsl(291.6, 100%, 50%), 59px 55px hsl(297, 100%, 50%), 58px 56px hsl(302.4, 100%, 50%), 58px 57px hsl(307.8, 100%, 50%), 58px 58px hsl(313.2, 100%, 50%); font-size: 60px; font-family:cursive;font-style: italic;";;
    console.log("%c%s", my.consoleCss, my.consoleMessage);
    
});

//On notification link click call
$("#notificationLink").click(function ()
{
    $("#notificationContainer").fadeToggle(300);
    return false;
});

//Document Click hiding the popup 
$(document).click(function ()
{
    $("#notificationContainer").hide();
});




