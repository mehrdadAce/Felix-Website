var config = {
    Environment: CFM.Environment.PROD,
    Culture: "nl-BE",
    SpinnerColor: CFM.SpinnerColor.GRAY
};
CFM.API.init(config);
CFM.API.enable("c592ae45-09b0-4e46-939d-1e36bb9fe7e6", "#vehicle-list");
CFM.API.enable("57f6594d-08a4-4a61-b0ca-a5972a3329ce", "#vehicle-search");