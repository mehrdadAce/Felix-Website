$(document).ready(function(){
    var date_input = $('input[name="date"]'); //our date input has the name "date"
    var container  = $('#datepicker');
    var options={
        format: 'dd/mm/yyyy',
        container: container,
        autoclose: true,
        startDate: "01/01/1990",
        endDate: "01/01/2000",
        maxViewMode: 3
    };
    date_input.datepicker(options);
});