angular.module('umbraco').controller('AcquisitionToolController',
    function ($scope, $location, appState, eventsService) {

        var screenWidth;
        var currentImage;

        $scope.acquisitionId = $location.absUrl().split("acquisitionId=")[1];

        function loadData() {
            loadOverview();
            loadAcquisition();
        }

        loadData();

        $scope.startup = function () {
            hideUmbracoNavigation();
            setWidthOfFlexboxes(); 
        };

        $(".wrapper").on('click', "img.imgSettings", showModal);

        $scope.checkKeycode = keyEvents;
        
        $scope.arrowRight = navigateToDirection;
        $scope.arrowLeft = navigateToDirection;

        // Modal JS
        var modal = document.getElementById('modal');

        // Get the image and insert it inside the modal - use its "alt" text as a caption
        function showModal() {
            var modalImg = document.getElementById("image");
            var captionText = document.getElementById("caption");
            hideUmbracoNavigation();
            modal.style.display = "block";
            modalImg.src = this.src;
            captionText.innerHTML = this.alt;
            modal.style.marginLeft = "0px";
            currentImage = this;
        }
        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks on <span> (x), close the modal
        span.onclick = function () {
            modal.style.display = "none";
        } 

        // Check if the arrow keys were pressed
        function keyEvents(keyEvent) {
            const leftKey = 37;
            const rightKey = 39;

            switch (keyEvent.which) {
                case leftKey:
                    navigateToDirection(keyEvent, 0);
                    break;
                case rightKey:
                    navigateToDirection(keyEvent, 1);
                    break;
            }
        }

        // Navigate template
        function navigateToDirection(event, direction) {
            var modalImg = document.getElementById("image");
            var captionText = document.getElementById("caption");
            var allImages = Array.prototype.slice.call($('.imgSettings'));
            var image = currentImage;
            var newImage;
            if (direction === 0) {
                newImage = navigateToPreviousImage(allImages, image);
            } else if (direction === 1) {
                newImage = navigateToNextImage(allImages, image);
            } else {
                newImage = currentImage;
            }
            if (newImage === undefined) return;
            currentImage = newImage;
            modalImg.src = newImage.src;
         
            captionText.innerHTML = newImage.alt;
        }
        
        // Navigate to the next image
        function navigateToNextImage(allImages, image) {
            var count = 0;
            var nextImage;
            allImages.forEach(im => {
                if (im.alt === image.alt) {
                    if (allImages.length === count + 1) {
                        nextImage = undefined;
                    } else {
                        nextImage = allImages[count + 1];
                    }
                }
                count++;
            });
            return nextImage;
        }

        // Navigate to the previous image
        function navigateToPreviousImage(allImages, image) {
            var count = 0;
            var previousImage;
            allImages.forEach(im => {
                if (im.alt === image.alt) {
                    if (allImages.length === count - 1) {
                        previousImage = undefined;
                    } else {
                        previousImage = allImages[count - 1];
                    }
                }
                count++;
            });
            return previousImage;
        }

        // Toggle the navigation panel
        function hideUmbracoNavigation() {

            //Get the current state of showNavigation
            var currentNavigationState = appState.getGlobalState('showNavigation');

            if (currentNavigationState === true) {
                //Toggle the tree visibility
                appState.setGlobalState("showNavigation", !currentNavigationState);
            }
        }

        function showUmbracoNavigation() {
            var currentNavigationState = appState.getGlobalState('showNavigation');

            if (currentNavigationState === false) {
                appState.setGlobalState("showNavigation", true);
            }
        }

        eventsService.on("appState.globalState.changed", function (e, args) {

            if (args.key === "showNavigation") {

                //If false (So hiding navigation)
                if (!args.value) {
                    //Set css left position to 80px (width of appBar)
                    document.getElementById("contentwrapper").style.left = "80px";
                }
                else {
                    //Remove the CSS we set so default CSS of Umbraco kicks in
                    document.getElementById("contentwrapper").style.left = "";
                }
            }
        });

        // Toggle the navigation panel end

        function loadOverview () {
            $.ajax({
                url: '/umbraco/backoffice/AcquisitionTool/GetOverviewData?id=' + $scope.acquisitionId,
                dataType: 'json',
                type:"GET",
                success: function (data) {
                    const resizeConfig = '?width=700&height=500';
                    $scope.overviewModel = data;

                    $scope.overviewModel.LeftFrontUrl += resizeConfig;
                    $scope.overviewModel.RightFrontUrl += resizeConfig;
                    $scope.overviewModel.LeftBackUrl += resizeConfig;
                    $scope.overviewModel.RightBackUrl += resizeConfig;
                    $scope.overviewModel.DashboardUrl += resizeConfig;

                    $scope.overviewModel.KmUrl += resizeConfig;
                    $scope.overviewModel.FrontseatsUrl += resizeConfig;
                    $scope.overviewModel.BackseatsUrl += resizeConfig;

                    $scope.overviewModel.DmgInsideUrls.forEach(function (dmgInsideUrl) {
                        dmgInsideUrl.DamageUrl += resizeConfig;
                    });

                    $scope.overviewModel.DmgOutsideUrls.forEach(function (dmgOutsideUrl) {
                        dmgOutsideUrl.DamageUrl += resizeConfig;
                    });

                    $scope.overviewModel.EnrollmentFrontUrl += resizeConfig;
                    $scope.overviewModel.EnrollmentBackUrl += resizeConfig;
                    $scope.overviewModel.ExaminationUrl += resizeConfig;

                    $scope.$apply();
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }

        function loadAcquisition() {
            $.ajax({
                url: "/umbraco/backoffice/AcquisitionTool/GetAcquisitionWithId?id=" + $scope.acquisitionId,
                dataType: 'json',
                success: function (data) {
                    var pattern = /Date\(([^)]+)\)/;
                    var results = pattern.exec(data.CreatedDate);
                    var dt = new Date(parseFloat(results[1]));
                    data.CreatedDate = dt;
                    
                    $scope.acquisitionData = data;
                    $scope.$apply();
                },
                error: function (error) {
                    console.log(error);
                }
            });
        };

        function setWidthOfFlexboxes() {
            screenWidth = $("#contentwrapper").width();
            $("#outsidePhotos").width(screenWidth);
        };

        $scope.downloadFiles = function () {
            $scope.acquisitionId = $location.url().split('acquisitionId=')[1];
            window.location = `/umbraco/backoffice/AcquisitionTool/DownloadAcquisitionFiles?id=${$scope.acquisitionId}&isTakeOver=true`;
        };
    });
