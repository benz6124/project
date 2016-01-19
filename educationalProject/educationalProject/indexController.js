'use strict';

app.controller('choice_index_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope) {

    // 



    // console.log(select_nothing);

     $scope.init_var = function(){

    $scope.not_select_curri_and_year = true;
    $scope.not_select_sub_indicator = true;
    $scope.year_choosen = {};
     $scope.curri_choosen={};
     $scope.indicator_choosen = {};
     $scope.sub_indicator_choosen = {};
     $scope.select_overall = true;
     $scope.select_year_support_text = 0;
     $scope.select_all_complete = false;
     $scope.already_select_curri = false;
     $scope.questions = [];
     $scope.show_preview_support_text = 0;
     $scope.current_section_save = [];

       $rootScope.all_curriculums = [];


     }

         $scope.add_question = function(){
            // console.log("welcome to add_question");
            var newItemNo = $scope.questions.length+1;
           $scope.questions.push({'id':newItemNo,'hide':false});
         }

         $scope.remove_question = function(question){
            question.hide = true;
          
         }

        // request_all_curriculums_service_server.async().then(function(data) {
        //     $rootScope.all_curriculums = data;

        //   });

        $http.get('/api/curriculum').success(function (data) {
            
            $rootScope.all_curriculums = data;
          });
            


    $scope.clear_select_year_support_text_choosen = function(){
        console.log("clear");
        $scope.select_year_support_text = 0;
    }
    $scope.choose_overall = function(){
        $scope.select_overall = true;
    }
    $scope.sendCurriAndGetYears = function (curri) {
        $scope.select_all_complete = false;
         $scope.not_select_curri_and_year = true;
         $scope.already_select_curri = true;
           $scope.year_choosen = {};
        // console.log("it's me");
      
        //    $http.post('/api/curriculumacademic',  {'Cu_curriculum': curri }).success(function (data, status, headers, config) {
        //     $scope.corresponding_aca_years = data;
        // });
      
        $http.post(
             '/api/curriculumacademic/getbycurriculum',
             JSON.stringify(curri),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.corresponding_aca_years = data;
         });
    }
    // $scope.loadingIndexPage = function(){
    //     $event = $scope.sendYearAndGetIndicators($scope.year_choosen);
    // }
    $scope.chooseYear = function(){
            $scope.select_all_complete = true;
    }

    $scope.check_curri = function(){
        // console.log("welcome check_curri");
        if ($scope.already_select_curri == false){
              $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาเลือกหลักสูตรที่ต้องการก่อน',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
                
        }
    }
     $scope.sendYearAndGetIndicators = function (year) {
        
        if ($scope.select_all_complete  == true){
         $scope.not_select_curri_and_year = false;
        $scope.select_overall = true;
        $scope.not_select_sub_indicator = true;
        // console.log(year);
        // console.log(year.aca_year);

        $http.post(
             '/api/indicator',
             JSON.stringify(year),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.corresponding_indicators = data;
         });

         }
         else if ($scope.already_select_curri == true){
            $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาเลือกปีการศึกษา',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
                

         }
         else{ 

            $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาเลือกหลักสูตรและปีการศึกษา',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
                

            // $alert('กรุณาเลือกหลักสูตรและปีการศึกษา','เกิดข้อผิดพลาด', 'danger', 'bottom-right')
         }
    }

    $scope.send_sub_indicator = function(sub_indicator){
        $scope.sub_indicator_choosen = sub_indicator;
        $scope.not_select_sub_indicator = false;
        $scope.sendSectionSaveAndGetSupportText();


    }
     // $scope.loading = new $loading({
     //        busyText: 'ระบบกำลังดำเนินการโหลด...',
     //        theme: 'success',
     //        timeout: false,
     //        delayHide: 1000,
     //        showSpinner:false
     //    });

    
     $scope.sendIndicatorAndGetSubIndicators = function (indicator) {
        // $scope.loading.show();
        $scope.not_select_sub_indicator = true;
          $scope.indicator_choosen = indicator;
          $scope.select_overall = false;


        $http.post(
             '/api/subindicator',
             JSON.stringify(indicator),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.corresponding_sub_indicators = data;
             $scope.sendIndicatorCurriAndGetEvaluation();
         });

    }
    
    $scope.sendIndicatorCurriAndGetEvaluation = function () {
        // $scope.loading = new $loading();
        // $scope.loading.show();
        $scope.indicator_choosen.curri_id = $scope.curri_choosen.curri_id;

        // console.log($scope.indicator_choosen);

        $http.post(
             '/api/evaluationresult',
             JSON.stringify($scope.indicator_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            // console.log(data);
             $scope.evaluation_result_receive = data;
             $scope.evaluation_result= [];
             var index;
             // console.log($scope.evaluation_result_receive);
             // console.log($scope.evaluation_result_receive.self.length);
            //  $scope.evaluation_result.teacher = []
            // $scope.evaluation_result.teacher.teacher_name = $scope.evaluation_result_receive.self[0].t_name;
            // $scope.evaluation_result.teacher.date = $scope.evaluation_result_receive.self[0].date;
            // $scope.evaluation_result.teacher.time = $scope.evaluation_result_receive.self[0].time;
            //  $scope.evaluation_result.assessor = [];
            //  $scope.evaluation_result.assessor.assessor_name = $scope.evaluation_result_receive.others[0].t_name;
            //   $scope.evaluation_result.assessor.date = $scope.evaluation_result_receive.others[0].date;
            //    $scope.evaluation_result.assessor.time = $scope.evaluation_result_receive.others[0].time;

             $scope.evaluation_result.results = {};
             for (index = 0; index < $scope.evaluation_result_receive.self.length; index++) {
                $scope.evaluation_result.results[index] = []
                $scope.evaluation_result.results[index].sub_indicator_name =  $scope.corresponding_sub_indicators[index].sub_indicator_name;
                $scope.evaluation_result.results[index].sub_indicator_num = $scope.evaluation_result_receive.self[index].sub_indicator_num;
                $scope.evaluation_result.results[index].self_result = $scope.evaluation_result_receive.self[index].evaluation_score;
                $scope.evaluation_result.results[index].other_result = $scope.evaluation_result_receive.others[index].evaluation_score;
         
            }

            $scope.sendIndicatorCurriAndGetEvidence();
         });
    }

    $scope.download_file = function(path) { 
        window.open(path, '_blank', '');  
    }




    $scope.sendIndicatorCurriAndGetEvidence = function () {

  

        console.log($scope.indicator_choosen);

        $http.post(
             '/api/evidence',
             JSON.stringify($scope.indicator_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            console.log(data);
            $scope.corresponding_evidences = data;
       
         });
    }

// $scope.confirm_support_text = function () {
//     CKEDITOR.instances['support_text'].setData(CKEDITOR.instances['support_text'].getData());
//     send_support_text_change_to_server
// }

$scope.watch_support_text_from_other_year= function(){
    console.log($scope.select_year_support_text);
if($scope.select_year_support_text != 0){
      ngDialog.open({
    template:$scope.show_preview_support_text,
    plain: true,
    className: 'ngdialog-theme-default',
    showClose :true,
    
});

  }
  else{
     $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาเลือกปีการศึกษา',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
                
  }
}
$scope.get_support_content_from_other_year = function () {
    CKEDITOR.instances['support_text'].setData($scope.show_preview_support_text);
    this.$hide();

  
    // alert( CKEDITOR.instances['support_text'].getData());
    // CKEDITOR.instances['support_text'].setData("cheese pizza");
}

$scope.prepare_to_watch_support_text = function(){
    console.log("prepare_to_watch_support_text");
    $scope.sendSectionSaveAndGetPreviewSupportText();
}

$scope.download_aun_book = function(){
         $http.post(
             '/api/aunbook',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.download_file(data);


         });
         // .error(function (data) {
            
         //        $alert({title:'เกิดข้อผิดพลาด', content:'ไม่พบข้อมูล',alertType:'success',
         //                 placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});


         // });
}
$scope.send_support_text_change_to_server = function(){
    console.log("send_support_text_change_to_server");
    $scope.current_section_save.detail = CKEDITOR.instances['support_text'].getData();
    $scope.current_section_save.teacher_id = "00010";
      $http.put(
             '/api/sectionsave',
             JSON.stringify( $scope.current_section_save),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
    
                $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

         });
}
 $scope.sendSectionSaveAndGetPreviewSupportText = function () {
 console.log("sendSectionSaveAndGetPreviewSupportText");
console.log($scope.select_year_support_text.aca_year);
     $scope.section_save_to_send = new Object();
     $scope.section_save_to_send.teacher_id = "";
     $scope.section_save_to_send.detail  = "";
     $scope.section_save_to_send.date  = "";
     $scope.section_save_to_send.time  = "";

     $scope.section_save_to_send.aca_year = $scope.select_year_support_text.aca_year;
     $scope.section_save_to_send.indicator_num  = $scope.indicator_choosen.indicator_num;
     $scope.section_save_to_send.sub_indicator_num   = $scope.sub_indicator_choosen.sub_indicator_num;
     $scope.section_save_to_send.curri_id = $scope.curri_choosen.curri_id;
        $http.post(
             '/api/sectionsave',
             JSON.stringify($scope.section_save_to_send),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
    console.log(data);
            $scope.show_preview_support_text= data.detail;

         });
    }

    // $scope.filter_not_selected_year =function(year){
  
    //     return $scope.year_choosen.aca_year != year.aca_year;
    // }


        $scope.sendSectionSaveAndGetSupportText = function () {

     $scope.section_save_to_send = new Object();
     $scope.section_save_to_send.teacher_id = "";
     $scope.section_save_to_send.detail  = "";
     $scope.section_save_to_send.date  = "";
     $scope.section_save_to_send.time  = "";
     $scope.section_save_to_send.aca_year = $scope.year_choosen.aca_year;
     $scope.section_save_to_send.indicator_num  = $scope.indicator_choosen.indicator_num;
     $scope.section_save_to_send.sub_indicator_num   = $scope.sub_indicator_choosen.sub_indicator_num;
     $scope.section_save_to_send.curri_id = $scope.curri_choosen.curri_id;
        $http.post(
             '/api/sectionsave',
             JSON.stringify($scope.section_save_to_send),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             console.log("sendSectionSaveAndGetSupportText");
            console.log(data);
            $scope.current_section_save = data;
            CKEDITOR.instances['support_text'].setData(data.detail);

         });
    }
});

app.controller('create_curriculum', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope) {
    $scope.init = function(){
        $scope.new_curri = []

    }
    $scope.create_curri = function(dada){
          console.log($scope.new_curri);
        // $scope.new_curri = {"curri_id":"19",
        // "year":"2546",
        // "curr_tname":"วิศวกรรมศาสตรบัณฑิต สาขาวิชาวิศวกรรมคอมพิวเตอร์ฉบับ พ.ศ.2546",
        // "curr_ename":"Curriculum for Bachelor of Engineering Program in Computer",
        // "degree_t_full":"วิศวกรรมศาสตรบัณฑิต (วิศวกรรมคอมพิวเตอร์)",
        // "degree_t_bf":"วศ.บ. (วิศวกรรมคอมพิวเตอร์)",
        // "degree_e_full":"Bachelor of Engineering (Computer Engineering)",
        // "degree_e_bf":"B.Eng. (Computer Engineering)",
        // "level":"1",
        // "period":"4"}

        $scope.new_curri.year= "";
         $http.post(
             '/api/curriculum',
             JSON.stringify($scope.new_curri),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             console.log("success");
                 console.log(data);
                 $rootScope.all_curriculums =data;
         //เรียกฟังชั่นใน servce ให้อัพเดทค่า


         });
    }
});
