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
        window.open(path, '_blank', "");  
    }


    $scope.watch_file = function(path) { 
        window.open(path, '_blank', "width=800, left=230,top=0,height=700");  
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
        console.log(data);
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

         })
    .error(function(data, status, headers, config) {
                  if(status==500){
     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
    
}
 $scope.sendSectionSaveAndGetPreviewSupportText = function () {
    if($scope.select_year_support_text!=0){
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
                 else{
                      $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาเลือกปีการศึกษาก่อน',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
                 }
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
app.controller('add_aca_year', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope) {
    $scope.init = function(){
        $scope.curri_choosen = "none";
               $scope.new_curri_academic = {};
        $scope.new_curri_academic.aca_year = ""
 
        $scope.error_leaw = false;
    
    }

    $scope.add_aca_year_to_server = function(){
        console.log("add_aca_year_to_server");
if( $scope.curri_choosen!= "none" && $scope.new_curri_academic.aca_year != ""){
        $scope.new_curri_academic.curri_id = $scope.curri_choosen.curri_id;
  console.log($scope.new_curri_academic);
              $http.post(
             '/api/curriculumacademic/add',
             JSON.stringify($scope.new_curri_academic),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            console.log("success");
                 console.log(data);
            
                  $alert({title:'ดำเนินการสำเร็จ', content:'เพิ่มปีการศึกษาเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
               
         

         })
         .error(function(data, status, headers, config) {
                  if(status==400){
     $alert({title:'เกิดข้อผิดพลาด', content:data.message,alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 

     }else{
       $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาเลือกหลักสูตรและปีการศึกษา',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});

     }
    }
});
app.controller('create_curriculum', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope) {
    $scope.init = function(){
        $scope.new_curri = {}

    }
    $scope.create_curri = function(){
          console.log($scope.new_curri);

        if ($scope.new_curri.curr_tname && $scope.new_curri.curr_ename && $scope.new_curri.degree_t_full && $scope.new_curri.degree_t_bf && $scope.new_curri.degree_e_full && $scope.new_curri.degree_e_bf && $scope.new_curri.level && $scope.new_curri.period ){
        // "year":"2546",
        // "curr_tname":"วิศวกรรมศาสตรบัณฑิต สาขาวิชาวิศวกรรมคอมพิวเตอร์ฉบับ พ.ศ.2546",
        // "curr_ename":"Curriculum for Bachelor of Engineering Program in Computer",
        // "degree_t_full":"วิศวกรรมศาสตรบัณฑิต (วิศวกรรมคอมพิวเตอร์)",
        // "degree_t_bf":"วศ.บ. (วิศวกรรมคอมพิวเตอร์)",
        // "degree_e_full":"Bachelor of Engineering (Computer Engineering)",
        // "degree_e_bf":"B.Eng. (Computer Engineering)",
        // "level":"1",
        // "period":"4")

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
                   $alert({title:'ดำเนินการสำเร็จ', content:'สร้างหลักสูตรเรียบร้อยแล้ว',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
         //เรียกฟังชั่นใน servce ให้อัพเดทค่า


         });
    }
    else{
          $alert({title:'เกิดข้อผิดพลาด', content:'กรุณากรอกข้อมูลให้ครบถ้วน',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
    }
}
});


app.controller('stat_graduated_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope) {
$scope.init =function() {
     $scope.choose_not_complete = true;
}
      $scope.year_choosen = {};
              $scope.curri_choosen = {}
       $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
    console.log($scope.curri_choosen);
      
        $http.post(
             '/api/curriculumacademic/getbycurriculum',
             JSON.stringify($scope.curri_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.corresponding_aca_years = data;
         });
    }

    $scope.find_information = function(){

          console.log("find_information");
        console.log($scope.year_choosen);

        $http.post(
             '/api/studentstatusother',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            
            console.log(data);
             $scope.result = data;
             $scope.choose_not_complete = false;
         });

    }

    $scope.save_to_server = function(){
        console.log("save_to_server");
        console.log($scope.result);
        $http.put(
             '/api/studentstatusother',
             JSON.stringify($scope.result),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
               $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
         })
    .error(function(data, status, headers, config) {
                  if(status==500){

     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
    }
});



app.controller('stat_student_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope) {
$scope.init =function() {
     $scope.choose_not_complete = true;
}
      $scope.year_choosen = {};
              $scope.curri_choosen = {}
       $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
    console.log($scope.curri_choosen);
      
        $http.post(
             '/api/curriculumacademic/getbycurriculum',
             JSON.stringify($scope.curri_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.corresponding_aca_years = data;
         });
    }

    $scope.find_information = function(){

          console.log("find_information");
        console.log($scope.year_choosen);

        $http.post(
             '/api/studentcount',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            
            console.log(data);
             $scope.result = data;
             $scope.choose_not_complete = false;
         });

    }

    $scope.save_to_server = function(){
        console.log("save_to_server");
        console.log($scope.result);
        $http.put(
             '/api/studentcount',
             JSON.stringify($scope.result),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
               $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
         })
    .error(function(data, status, headers, config) {
                  if(status==500){
                    
     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
    }
});



app.controller('stat_new_student_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope) {
$scope.init =function() {
     $scope.choose_not_complete = true;
}
      $scope.year_choosen = {};
              $scope.curri_choosen = {}
       $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
    console.log($scope.curri_choosen);
      
        $http.post(
             '/api/curriculumacademic/getbycurriculum',
             JSON.stringify($scope.curri_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.corresponding_aca_years = data;
         });
    }

    $scope.find_information = function(){

          console.log("find_information");
        console.log($scope.year_choosen);

        $http.post(
             '/api/newstudentcount',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            
            console.log(data);
             $scope.result = data;
             $scope.choose_not_complete = false;
         });

    }

    $scope.save_to_server = function(){
        console.log("save_to_server");
        console.log($scope.result);
        $http.put(
             '/api/newstudentcount',
             JSON.stringify($scope.result),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
               $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
         })
    .error(function(data, status, headers, config) {
                  if(status==500){
                    
     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
    }
});

app.directive('fileUpload', function () {
    return {
        scope: true,        //create a new scope
        link: function (scope, el, attrs) {
            el.bind('change', function (event) {
                var files = event.target.files;
                //iterate files since 'multiple' may be specified on the element
                for (var i = 0;i<files.length;i++) {
                    //emit event upward
                    scope.$emit("fileSelected", { file: files[i] });
                }                                       
            });
        }
    };
});

app.controller('evaluate_by_me_controller', function ctrl($scope, $alert,$http) {
$scope.init =function() {
     $scope.choose_not_complete = true;

}
      $scope.year_choosen = {};
       
$scope.indicator_choosen = {};

     $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
    console.log($scope.curri_choosen);
      
        $http.post(
             '/api/curriculumacademic/getbycurriculum',
             JSON.stringify($scope.curri_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.corresponding_aca_years = data;
         });
    }

    $scope.find_indicators = function(){

          console.log("find_indicators");
        console.log($scope.year_choosen);

        $http.post(
             '/api/indicator',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
     
             $scope.corresponding_indicators = data;
         });

    }

    $scope.get_results= function(){
        console.log($scope.indicator_choosen);
        $scope.indicator_choosen.curri_id = $scope.curri_choosen.curri_id ;
        $http.post(
             '/api/selfevaluation',
             JSON.stringify($scope.indicator_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
                   console.log("wait results");
            console.log(data);
 
             $scope.corresponding_results = data;
             $scope.choose_not_complete = false;
             $scope.mock_result = data[0];
         });

    }

    $scope.save_to_server = function(){

        $http.post(
             '/api/indicator',
             JSON.stringify($scope.corresponding_results),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
              $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
         })
         .error(function (data, status, headers, config) {
            $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
    }
});

app.controller('upload_aun_controller', function ctrl($scope, $alert,$http) {

    $scope.init =function() {
        console.log("init");
     $scope.choose_not_complete = true;
           $scope.year_choosen = {};
              $scope.curri_choosen = {}
  $scope.files = [];
  
}

  $scope.find_information = function(){

      
          $scope.choose_not_complete = false;
    }

    $scope.file_not_already_upload = function(){

        return $scope.files.length==0;
    }
       $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
    console.log($scope.curri_choosen);
      
        $http.post(
             '/api/curriculumacademic/getbycurriculum',
             JSON.stringify($scope.curri_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.corresponding_aca_years = data;
         });
    }


    //a simple model to bind to and send to the server




    //listen for the file selected event
    $scope.$on("fileSelected", function (event, args) {
        $scope.$apply(function () {            
            //add the file object to the scope's files collection
            $scope.files.push(args.file);
        });
    });
    
    $scope.save_to_server = function() {
 
      $scope.model  = {"file_name":$scope.files[0].name,"personnel_id":"00007","date":"","curri_id":$scope.curri_choosen.curri_id,"aca_year":$scope.year_choosen.aca_year}

      var formData = new FormData();

    formData.append("model", angular.toJson($scope.model));

        for (var i = 0; i < $scope.files.length; i++) {
        
            formData.append("file" + i, $scope.files[i]);
        }

        $http({
            method: 'PUT',
            url: "/Api/aunbook",

            headers: { 'Content-Type': undefined },


            data:formData,
            transformRequest: angular.indentity 

        }).
        success(function (data, status, headers, config) {
    
                $scope.files = [];
             

                $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
           
        }).
        error(function (data, status, headers, config) {
            $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
    };
});

app.controller('manage_president_controller', function ctrl($scope, $alert,$http) {
$scope.init =function() {
     $scope.choose_not_complete = true;
}
      $scope.year_choosen = {};
              $scope.curri_choosen = {}
              
       $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
    console.log($scope.curri_choosen);
      
        $http.post(
             '/api/curriculumacademic/getbycurriculum',
             JSON.stringify($scope.curri_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.corresponding_aca_years = data;
         });
    }

    $scope.find_information = function(){

          console.log("find_information");
        console.log($scope.year_choosen);

        $http.post(
             '/api/newstudentcount',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            
            console.log(data);
             $scope.result = data;
             $scope.choose_not_complete = false;
         });

    }

    $scope.save_to_server = function(){
        console.log("save_to_server");
        console.log($scope.result);
        $http.put(
             '/api/newstudentcount',
             JSON.stringify($scope.result),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
               $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
         })
    .error(function(data, status, headers, config) {
                  if(status==500){
                    
     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
    }
});
 // $scope.save = function() {

 //      $scope.model  = {"file_name":"","personnel_id":"00007","date":"","curri_id":curri_choosen.curri_id,"aca_year":year_choosen.aca_year}

 //      var formData = new FormData();

 //    formData.append("model", angular.toJson($scope.model));

 //        for (var i = 0; i < $scope.files.length; i++) {
        
 //            formData.append("file" + i, $scope.files[i]);
 //        }

 //        $http({
 //            method: 'POST',
 //            url: "/Api/aunbook",

 //            headers: { 'Content-Type': undefined },


 //            data:formData,
 //            transformRequest: angular.indentity 

 //        }).
 //        success(function (data, status, headers, config) {
 //            alert("success!");
 //        }).
 //        error(function (data, status, headers, config) {
 //            alert("failed!");
 //        });