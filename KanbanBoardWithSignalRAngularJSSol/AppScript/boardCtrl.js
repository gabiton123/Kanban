sulhome.kanbanBoardApp.controller('boardCtrl', function ($scope, boardService) {
    // Model
    $scope.columns = [];
    $scope.isLoading = false;

    function init() {
        $scope.isLoading = true;
        boardService.initialize().then(function (data) {
            $scope.isLoading = false;
            $scope.refreshBoard();
        }, onError);
    };

    $scope.refreshBoard = function refreshBoard() {        
        $scope.isLoading = true;
        boardService.getColumns()
           .then(function (data) {               
               $scope.isLoading = false;
               $scope.columns = data;
           }, onError);
    };    

    $scope.onDrop = function (data, targetColId) {        
        boardService.canMoveTask(data.ColumnId, targetColId)
            .then(function (canMove) {                
                if (canMove) {                 
                    boardService.moveTask(data.Id, targetColId).then(function (taskMoved) {
                        $scope.isLoading = false;                        
                        boardService.sendRequest();
                    }, onError);
                    $scope.isLoading = true;
                }

            }, onError);
    };

    // Listen to the 'refreshBoard' event and refresh the board as a result
    $scope.$parent.$on("refreshBoard", function (e) {
        $scope.refreshBoard();
        toastr.success("Board updated successfully", "Success");
    });

    
    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };

    // Get the modal
    var modal = document.getElementById("myModal");

    // Get the button that opens the modal
    var btn = document.getElementById("myBtn");

    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];

    // When the user clicks the button, open the modal 
    btn.onclick = function () {
        modal.style.display = "block";
    }

    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
    }

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
    init();
});
