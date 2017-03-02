$(document).ready(function() {

    my.webWorker = function() {

        var worker;

        var startWorker = function(filePath, callback) {
            if (typeof (Worker) !== "undefined") {
                if (typeof (worker) == "undefined" || filePath != null) {// cannot create a new webworker if last worker is not terminated
                    worker = new Worker(filePath);
                }
                worker.onmessage = function (event) {
                     callback(event.data);
                };
                worker.onerror = function(evt) {
                     alert(evt.message);
                }
            } else {
                alert("Sorry! No Web Worker support.");
            }
        }

        var stopWorker = function() {
            worker.terminate();
            worker = undefined;
        }
        
        return{
            startWorker: startWorker,
            stopWorker: stopWorker,
            worker : worker
        }
    }();


});