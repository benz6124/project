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

        request_all_curriculums_service_server.async().then(function(data) {
            $rootScope.all_curriculums = data;

          });

        // $http.get('/api/curriculum').success(function (data) {
            
        //     $rootScope.all_curriculums = data;
        //   });
            


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
             '/api/indicator/querybycurriculumacademic',
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
            console.log("this is evaluationresult");
            console.log(data);
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

            if ($scope.evaluation_result_receive.self.length>$scope.evaluation_result_receive.others.length)
                {

                for (index = 0; index < $scope.evaluation_result_receive.self.length; index++) {
                    console.log(index);
                    $scope.evaluation_result.results[index] = []
                    $scope.evaluation_result.results[index].sub_indicator_name =  $scope.corresponding_sub_indicators[index].sub_indicator_name;
                    $scope.evaluation_result.results[index].sub_indicator_num = $scope.evaluation_result_receive.self[index].sub_indicator_num;
                    $scope.evaluation_result.results[index].self_result = $scope.evaluation_result_receive.self[index].evaluation_score;
                    // $scope.evaluation_result.results[index].other_result = $scope.evaluation_result_receive.others[index].evaluation_score;
                      }
                      $scope.self_not_evaluated = false;
                     $scope.others_not_evaluated = true;
                     $scope.evaluation_self_date = $scope.evaluation_result_receive.self[0].date;
                     $scope.evaluation_self_time =  $scope.evaluation_result_receive.self[0].time;
                     $scope.evaluation_self_teacher = $scope.evaluation_result_receive.self[0].t_name;
                     $scope.evaluation_others_date = "";
                     $scope.evaluation_others_time = "";
                     $scope.evaluation_others_assessor = "";
              

                    }
                else if($scope.evaluation_result_receive.self.length<$scope.evaluation_result_receive.others.length){
                    $scope.self_not_evaluated = true;
                     $scope.others_not_evaluated = false;
                       $scope.evaluation_self_date = "";
                     $scope.evaluation_self_time =  "";
                     $scope.evaluation_self_teacher = "";
                    $scope.evaluation_others_date = $scope.evaluation_result_receive.others[0].date;
                     $scope.evaluation_others_assessor = $scope.evaluation_result_receive.others[0].time;
                     $scope.evaluation_others_time = $scope.evaluation_result_receive.others[0].t_name;
                     for (index = 0; index < $scope.evaluation_result_receive.others.length; index++) {
                    console.log(index);
                    $scope.evaluation_result.results[index] = []
                    $scope.evaluation_result.results[index].sub_indicator_name =  $scope.corresponding_sub_indicators[index].sub_indicator_name;
                    $scope.evaluation_result.results[index].sub_indicator_num = $scope.evaluation_result_receive.others[index].sub_indicator_num;

                    $scope.evaluation_result.results[index].other_result = $scope.evaluation_result_receive.others[index].evaluation_score;
                }
                }else{
                 for (index = 0; index < $scope.evaluation_result_receive.self.length; index++) {
                    console.log(index);
                    $scope.evaluation_result.results[index] = []
                    $scope.evaluation_result.results[index].sub_indicator_name =  $scope.corresponding_sub_indicators[index].sub_indicator_name;
                    $scope.evaluation_result.results[index].sub_indicator_num = $scope.evaluation_result_receive.self[index].sub_indicator_num;
                    $scope.evaluation_result.results[index].self_result = $scope.evaluation_result_receive.self[index].evaluation_score;
                    $scope.evaluation_result.results[index].other_result = $scope.evaluation_result_receive.others[index].evaluation_score;
                        }

                             $scope.self_not_evaluated = false;
                     $scope.others_not_evaluated = false;
                      $scope.evaluation_self_date = $scope.evaluation_result_receive.self[0].date;
                     $scope.evaluation_self_time =  $scope.evaluation_result_receive.self[0].time;
                     $scope.evaluation_self_teacher = $scope.evaluation_result_receive.self[0].t_name;
                     $scope.evaluation_others_date = $scope.evaluation_result_receive.others[0].date;
                     $scope.evaluation_others_assessor = $scope.evaluation_result_receive.others[0].time;
                     $scope.evaluation_others_time = $scope.evaluation_result_receive.others[0].t_name;
              
                }
       console.log( $scope.self_not_evaluated);
                      console.log( $scope.others_not_evaluated);
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
        $scope.indicator_choosen.curri_id = $scope.curri_choosen.curri_id;
        $http.post(
             '/api/evidence/getnormal',
             JSON.stringify($scope.indicator_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            console.log("/api/evidence/getnormal");
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
  $scope.curri_choosen = "none";
               $scope.new_curri_academic = {};
        $scope.new_curri_academic.aca_year = ""
 
        $scope.error_leaw = false;
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.add_aca_year_to_server = function(my_modal){
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

                  my_modal.$hide();
               $scope.init();
         

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
 $scope.new_curri = {}

 
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }


    $scope.create_curri = function(my_modal){
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
                   my_modal.$hide();
                    $scope.new_curri = {};
         //เรียกฟังชั่นใน servce ให้อัพเดทค่า


         });
    }
    else{
          $alert({title:'เกิดข้อผิดพลาด', content:'กรุณากรอกข้อมูลให้ครบถ้วน',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
    }
}
});


app.controller('stat_graduated_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
}
  
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen).then(function(data) {

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
            
              $scope.result = data;
             $scope.choose_not_complete = false;
             
              var value;
              var key;
             if ($scope.result.num_admis_f==-1){
             
                angular.forEach($scope.result, function(value, key) {
                 
                  
                  if(key !="year" && key != "curri_id"){
                   
                    $scope.result[key] = "";
                  }
                });
             }
         });

    }
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
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
               $scope.close_modal(my_modal);
         })
    .error(function(data, status, headers, config) {
                  if(status==500){

     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
    }
});



app.controller('stat_student_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {}
               $scope.indicator_choosen= {};
                  $scope.result ={};
}
   
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen).then(function(data) {

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
            
               $scope.result = data;
             $scope.choose_not_complete = false;
             
              var value;
              var key;
             if ($scope.result.num_admis_f==-1){
             
                angular.forEach($scope.result, function(value, key) {
                 
                  
                  if(key !="year" && key != "curri_id"){
                   
                    $scope.result[key] = "";
                  }
                });
             }
         });

    }

    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.save_to_server = function(my_modal){
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
                $scope.close_modal();
         })
    .error(function(data, status, headers, config) {
                  if(status==500){
                    
     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
    }
});



app.controller('stat_new_student_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
       $scope.year_choosen = {};
              $scope.curri_choosen = {};
                 $scope.indicator_choosen= {};
                   $scope.result = {};

}
    
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen).then(function(data) {

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
            
            
             $scope.result = data;
             $scope.choose_not_complete = false;
             
              var value;
              var key;
             if ($scope.result.num_admis_f==-1){
             
                angular.forEach($scope.result, function(value, key) {
                 
                  
                  if(key !="year" && key != "curri_id"){
                   
                    $scope.result[key] = "";
                  }
                });
             }
         });

    }

    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.save_to_server = function(my_modal){
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
               $scope.close_modal(my_modal);
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

app.controller('evaluate_by_me_controller', function($scope, $alert,$http,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
   $scope.year_choosen = {};
       $scope.curri_choosen = {};
$scope.indicator_choosen = {};
}
       $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

      $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen).then(function(data) {

            $scope.corresponding_aca_years = data;

          });


    }


    $scope.find_indicators = function(){

          console.log("find_indicators");
        console.log($scope.year_choosen);
$scope.indicator_choosen = {};
        $http.post(
             '/api/indicator/querybycurriculumacademic',
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

    $scope.save_to_server = function(my_modal){

        $http.put(
             '/api/selfevaluation',
             JSON.stringify($scope.corresponding_results),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
              $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
              my_modal.$hide();
              $scope.init();
         })
         .error(function (data, status, headers, config) {
            $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
    }
});


app.controller('evaluate_by_other_controller', function($scope, $alert,$http,request_years_from_curri_choosen_service) {
    $scope.init =function() {
 
     $scope.choose_not_complete = true;
           $scope.year_choosen = {};
              $scope.curri_choosen = {}
  $scope.files = [];
     $scope.year_choosen = {};
       
$scope.indicator_choosen = {};
}


   

  $scope.$on("fileSelected", function (event, args) {
        $scope.$apply(function () {            
            //add the file object to the scope's files collection
            $scope.files.push(args.file);
        });
    });
 $scope.file_not_already_upload = function(){

        return $scope.files.length==0;
    }
     $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen).then(function(data) {

            $scope.corresponding_aca_years = data;

          });


    }

$scope.clear_files = function(){
    $scope.files = [];
}


    $scope.find_indicators = function(){
$scope.indicator_choosen = {};
          console.log("find_indicators");
        console.log($scope.year_choosen);

        $http.post(
             '/api/indicator/querybycurriculumacademic',
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
     
         });

    }


    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

 $scope.save_to_server = function(my_modal) {

      var formData = new FormData();

    formData.append("model", angular.toJson($scope.corresponding_results));

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
              $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
              my_modal.$hide();
                  $scope.init ();
        }).
        error(function (data, status, headers, config) {
               $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
}

    // $scope.save_to_server = function(){

    //     $http.put(
    //          '/api/selfevaluation',
    //          JSON.stringify($scope.corresponding_results),
    //          {
    //              headers: {
    //                  'Content-Type': 'application/json'
    //              }
    //          }
    //      ).success(function (data) {
    //           $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
    //                      placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

    //           $scope.files=[];
    //      })
    //      .error(function (data, status, headers, config) {
    //         $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
    //                      placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
    //     });
    // }
});

app.controller('upload_aun_controller', function($scope, $alert,$http,request_years_from_curri_choosen_service) {

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
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen).then(function(data) {

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
    

        $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }


    $scope.save_to_server = function(my_modal) {
 
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
    
             

                $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

                $scope.close_modal(my_modal);
           
        }).
        error(function (data, status, headers, config) {
            $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
    };
});

app.controller('manage_president_controller', function($scope, $alert,$http,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                $scope.results={};
                $scope.my_president ={};
                       $scope.personnel_choose = {};
}
        $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                $scope.results={};
                $scope.my_president ={};
                $scope.personnel_choose = {};
// $scope.results =[

// {"history":[{"personnel_id":"00001","degree":"1","major":"ce","pre_major":"วศ.บ.","grad_year":2558,"college":"sdsd"},{"personnel_id":"00001","degree":"2","major":"ce1","pre_major":"วศ.บ.","grad_year":2559,"college":"sdsds"}],
// "teacher_id":"00001","degree":"1","position":"1","personnel_type":"1","person_id":"1","status":"normal","alive":49,"is_admin":"1","username":"kpwiboon","password":"1111111","user_type":"teacher","t_prename":"mr.","t_name":"wiboonth","e_prename":"mr.","e_name":"wiboon","citizen_id":"12448","gender":"m","email":"kpwiboon","tel":"000000","addr":"sd","file_name_pic":"บัณฑิต.jpg","timestamp":"2016-01-22T00:00:00+07:00"},

// {"history":[{"personnel_id":"00002","degree":"1","major":"ce22222","pre_major":"วศ.บ.","grad_year":2558,"college":"sdsd"},{"personnel_id":"00002","degree":"2","major":"ce0000","pre_major":"วศ.บ.","grad_year":2560,"college":"sdsds"}],"teacher_id":"00002","degree":"2","position":"1","personnel_type":"1","person_id":"1","status":"normal","alive":50,"is_admin":"0","username":"kwakaarad","password":"10111","user_type":"teacher","t_prename":"mr.","t_name":"akaaradth","e_prename":"mr.","e_name":"akaara","citizen_id":"1248","gender":"m","email":"kwakkarad","tel":"000000","addr":"sd","file_name_pic":"อรทัย.jpg","timestamp":"2016-01-22T00:00:00+07:00"}];


  $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen).then(function(data) {

            $scope.corresponding_aca_years = data;

          });


    }


    $scope.find_information = function(){

          console.log("find_information");
        console.log($scope.year_choosen);

        $http.post(
             '/api/PresidentCurriculum',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.choose_not_complete = false;
            $scope.results = data;
            $scope.personnel_choose = $scope.results[0];



            
         });


    }


    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.save_to_server = function(my_modal){
        console.log("save_to_server");
        // console.log($scope.personnel_choose);
        $scope.new_obj_to_send ={};

        $scope.new_obj_to_send.teacher_id = $scope.personnel_choose.teacher_id;
        $scope.new_obj_to_send.curri_id = $scope.curri_choosen.curri_id;
        $scope.new_obj_to_send.aca_year = $scope.year_choosen.aca_year;
        $http.put(
             '/api/PresidentCurriculum',
             JSON.stringify($scope.new_obj_to_send),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
               $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

               $scope.close_modal(my_modal);
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


app.controller('manage_indicators_controller', function($scope, $alert,$http,$rootScope){
  


      // $rootScope.manage_indicators_and_sub_result = [{"sub_indicator_list":[{"aca_year":2559,"indicator_num":1,"sub_indicator_num":1,"sub_indicator_name":"ข้อย่อย 1"}
      // ,{"aca_year":2559,"indicator_num":1,"sub_indicator_num":2,"sub_indicator_name":"ข้อย่อย 2"}
      // ,{"aca_year":2559,"indicator_num":1,"sub_indicator_num":3,"sub_indicator_name":"ข้อย่อย 3"}]
      // ,"aca_year":2559,"indicator_num":1,"indicator_name_t":"ชื่อไทย","indicator_name_e":"Eng name"}
      // ,{"sub_indicator_list":[{"aca_year":2559,"indicator_num":2,"sub_indicator_num":1,"sub_indicator_name":"ข้อย่อย 1"},{"aca_year":2559,"indicator_num":2,"sub_indicator_num":2,"sub_indicator_name":"ข้อย่อย 2"}],"aca_year":2559,"indicator_num":2,"indicator_name_t":"ชื่อไทย2","indicator_name_e":"Eng name2"}];
$scope.init = function(){
     $scope.choose_not_complete = true;
     $scope.year_choosen = 0;

         $scope.choose_not_complete = true;
      $rootScope.manage_indicators_and_subs_year_choosen = 0;
      

      $scope.curri_choosen = {};

    $rootScope.my_backup_indicators = {};
$rootScope.manage_indicators_year_to_create = "";
    $scope.year_to_create = "";

      $http.get('/api/curriculumacademic/getmaxacayear').success(function (data) {
            console.log("max_year_curri_have");
             $scope.max_year_curri_have = data;
             console.log(data);
           });
   $http.get('/api/indicator').success(function (data) {
            console.log("all_indicator_years");
console.log(data);
            $scope.all_indicator_years = data;
          });

 }


   $http.get('/api/indicator').success(function (data) {
            console.log("all_indicator_years");
console.log(data);
            $scope.all_indicator_years = data;
          });

     $http.get('/api/curriculumacademic/getmaxacayear').success(function (data) {
            console.log("max_year_curri_have");
             $scope.max_year_curri_have = data;
             console.log(data);
           });


    $scope.year_to_create = ""; 
    $scope.choose_not_complete = true;
    $scope.year_choosen = 0;
    $scope.choose_not_complete = true;
    $rootScope.manage_indicators_and_subs_year_choosen = 0;
    $scope.curri_choosen = {};
    $rootScope.my_backup_indicators = {};

    $rootScope.manage_indicators_year_to_create = "";
      $scope.recover_indicator_all =function(){
            $rootScope.manage_indicators_and_sub_result = angular.copy($rootScope.my_backup_indicators);

      }

     

        // $scope.tell_root_to_change = function(){
        //     console.log("tell");
          
        //     console.log($scope.year_choosen);
        //     console.log($rootScope.manage_indicators_and_subs_year_choosen);
        // }

        $scope.validate_year_to_create = function(){
            // console.log("this is value");
            // console.log($scope.year_to_create);
            return   angular.isUndefined($scope.year_to_create) || $scope.year_to_create <= $scope.max_year_curri_have || $scope.year_to_create == "";
        }
      $scope.choose_indicator = function(in_indi){
if( $scope.validate_year_to_create() != true){
        console.log("receive");
        console.log(in_indi);
        $rootScope.manage_indicators_and_sub_save_indicator = {};
        $rootScope.manage_indicators_and_sub_save_indicator.save_content = [];
        $rootScope.manage_indicators_and_sub_save_indicator.save_index = 0;
        console.log("choose_indicator");
            $rootScope.manage_indicators_indicator_choosen = in_indi;
           $rootScope.manage_indicators_and_sub_save_indicator.save_content =  angular.copy(in_indi);
            console.log('index');
            console.log( $rootScope.manage_indicators_and_sub_result.indexOf(in_indi));
             console.log($rootScope.manage_indicators_and_sub_save_indicator.save_index);
          

            $rootScope.manage_indicators_and_sub_save_indicator.save_index = angular.copy($rootScope.manage_indicators_and_sub_result.indexOf(in_indi));
       

        console.log("my back_up")
         console.log($rootScope.manage_indicators_and_sub_save_indicator.save_content);
         $rootScope.manage_indicators_year_to_create = $scope.year_to_create;

         }
         else{
             $alert({title:'เกิดข้อผิดพลาด', content:'กรุณากรอกปีการศึกษาให้ถูกต้อง',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
    
         }
      }



        

              $scope.still_not_choose_complete =function(){


if($scope.choose_not_complete==false){
var index;

$scope.my_num_indicators = [];
for (index =0;index<  $rootScope.manage_indicators_and_sub_result.length ; index++){

    if($scope.my_num_indicators.indexOf($rootScope.manage_indicators_and_sub_result[index].indicator_num) == -1) {
        $scope.my_num_indicators.push($rootScope.manage_indicators_and_sub_result[index].indicator_num);
    }
    else {

       return true;

    }


     if( $rootScope.manage_indicators_and_sub_result[index].indicator_name_t == "" || $rootScope.manage_indicators_and_sub_result[index].indicator_name_e == "" ||$rootScope.manage_indicators_and_sub_result[index].indicator_num == "" ){
          return true;

              }

}
          

       }

        return false;
      }


      $scope.add_indicator = function(){
        // $scope.new_indicator = {"sub_indicator_list":[]
        // ,"aca_year":$scope.year_choosen
        // ,"indicator_num":$rootScope.manage_indicators_and_sub_result.length+1
        // ,"indicator_name_t":"","indicator_name_e":""}


         $rootScope.manage_indicators_and_sub_result.push({"sub_indicator_list":[]
        ,"aca_year":$rootScope.year_to_create
        ,"indicator_num":""
        ,"indicator_name_t":"","indicator_name_e":""});
      }

      $scope.remove_indicator = function(index_indicator_to_remove) { 
      $rootScope.manage_indicators_and_sub_result.splice(index_indicator_to_remove, 1);     
      console.log($rootScope.manage_indicators_and_sub_result);
    }

    $scope.get_indicators = function(){
          $rootScope.manage_indicators_and_subs_year_choosen = $scope.year_choosen;
$scope.choose_not_complete = false;
console.log($rootScope.manage_indicators_and_subs_year_choosen);
          $http.post(
             '/api/indicatorsubindicator',
             JSON.stringify($rootScope.manage_indicators_and_subs_year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            console.log("get_indicators");
            console.log(data);
             $rootScope.my_backup_indicators = angular.copy(data);
            $rootScope.manage_indicators_and_sub_result =data;
         });
    }


    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }


        $scope.save_to_server = function(my_modal){


   
        var index;
          var sub_index;
 for (index = 0; index < $rootScope.manage_indicators_and_sub_result.length; index++) {
    $rootScope.manage_indicators_and_sub_result[index].aca_year = $scope.year_to_create;

    if(angular.isUndefined($rootScope.manage_indicators_and_sub_result[index].sub_indicator_list)){
        $rootScope.manage_indicators_and_sub_result[index].sub_indicator_list = [];
    }

    for(sub_index =0; sub_index < $rootScope.manage_indicators_and_sub_result[index].sub_indicator_list.length ; sub_index++ ){
        $rootScope.manage_indicators_and_sub_result[index].sub_indicator_list[sub_index].aca_year =$scope.year_to_create; 
    }
 }


if ($rootScope.manage_indicators_and_sub_result.length == 0){
    $rootScope.manage_indicators_and_sub_result = [];
   
    $rootScope.manage_indicators_and_sub_result.push({'aca_year':$scope.year_to_create});
}
      console.log("save_to_server");
        console.log($rootScope.manage_indicators_and_sub_result);
//         angular.forEach($rootScope.manage_indicators_and_sub_result,  function(value, key) {

//   this.push(key + ': ' + value);
// });


        $http.put(
             '/api/indicatorsubIndicator/saveindicator',
             JSON.stringify($rootScope.manage_indicators_and_sub_result),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {

               $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
                // $rootScope.manage_indicators_and_sub_result[$rootScope.manage_indicators_and_sub_save_indicator.save_index] = angular.copy($rootScope.manage_indicators_indicator_choosen);
                $rootScope.my_backup_indicators = angular.copy($rootScope.manage_indicators_and_sub_result);
                    $scope.close_modal(my_modal);
                // $rootScope.manage_indicators_and_sub_save_indicator.save_content = angular.copy($rootScope.manage_indicators_indicator_choosen);
         })
    .error(function(data, status, headers, config) {
                  if(status==500){

     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
    }


});


app.controller('change_evidence_file_controller', function($scope, $alert,$http,request_years_from_curri_choosen_service,$rootScope) {
      

       $scope.init =function() {
    
       $scope.my_temp_secret = false;
             
      $scope.files = [];
      
    }
   $scope.files = [];

    
       $scope.my_temp_secret = false;
    
    $scope.$on("fileSelected", function (event, args) {
        $scope.$apply(function () {    
            $scope.files =[];        
            //add the file object to the scope's files collection
            $scope.files.push(args.file);
        });
    });


         $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }


    $scope.save_to_server=function(this_modal){
 if($scope.my_temp_secret == false){
            $rootScope.only_object_want_to_change.secret = 0;
        }
        else{
            $rootScope.only_object_want_to_change.secret = 1;
        }




$rootScope.only_object_want_to_change.teacher_id = "00008";
      var formData = new FormData();
$rootScope.only_object_want_to_change.file_name = $scope.files[0].name;
    
    formData.append("model", angular.toJson($rootScope.only_object_want_to_change));

            formData.append("file" , $scope.files[0]);
        console.log("save to sserver");
console.log($rootScope.only_object_want_to_change);
        // $http({
        //     method: 'PUT',
        //     url: "/Api/aunbook",

        //     headers: { 'Content-Type': undefined },


        //     data:formData,
        //     transformRequest: angular.indentity 

        // }).
        // success(function (data, status, headers, config) {
    
           //  $rootScope.manage_evidences_world_evidences = data;

        //         $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
        //                  placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

        //         $scope.close_modal(my_modal);
           
        // }).
        // error(function (data, status, headers, config) {
        //     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
        //                  placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        // });
    }
});

app.controller('add_new_evidence_controller', function($scope, $alert,$http,$rootScope,request_years_from_curri_choosen_service){
 
    $scope.my_new_evidence = {};
   $scope.my_new_evidence.evidence_real_code = "";
   $scope.my_new_evidence.evidence_name = "";
   $scope.my_new_evidence.secret = false;
   $scope.my_new_evidence_file = [];
 $scope.my_temp_secret_new = false;
  $scope.my_new_evidence.teacher_id = "00007";
    $scope.init =function() {
         $scope.my_temp_secret_new = false;
   $scope.my_new_evidence = {};
   $scope.my_new_evidence.evidence_real_code = "";
   $scope.my_new_evidence.evidence_name = "";
   $scope.my_new_evidence_file = [];
     $scope.my_new_evidence.secret = false;
      $scope.my_new_evidence.evidence_name = "";
        $scope.my_new_evidence.teacher_id = "00007";
}

    $scope.$on("fileSelected", function (event, args) {
        $scope.$apply(function () {            
            $scope.my_new_evidence_file = [];
            //add the file object to the scope's files collection
            $scope.my_new_evidence_file.push(args.file);
            console.log("fileSelected");
            console.log($scope.my_new_evidence_file);
        });
    });



    $scope.new_evidence_still_not_complete =  function(){
        if (angular.isUndefined($scope.my_new_evidence.evidence_real_code) == true || angular.isUndefined($scope.my_new_evidence.evidence_name) == true || $scope.my_new_evidence_file.length ==0){

                return true;
        }
        else{
             if ($scope.my_new_evidence.evidence_real_code <= 0){
                   return true;
                }
            return false;
        }
    }

 $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.save_to_server =function(){
        if($scope.my_temp_secret_new  == false){
            $scope.my_new_evidence.secret = 0;
        }
        else{
            $scope.my_new_evidence.secret = 1;
        }



      var formData = new FormData();
$scope.my_new_evidence.file_name = $scope.my_new_evidence_file[0].name;
$scope.my_new_evidence.curri_id =   $rootScope.manage_evidence_curri_id_now;
$scope.my_new_evidence.aca_year = $rootScope.manage_evidence_year_now;



    
    formData.append("model", angular.toJson($scope.my_new_evidence));
    formData.append("file" , $scope.my_new_evidence_file[0]);
             console.log("save to sserver");
console.log($scope.my_new_evidence);

        // $http({
        //     method: 'PUT',
        //     url: "/Api/aunbook",

        //     headers: { 'Content-Type': undefined },


        //     data:formData,
        //     transformRequest: angular.indentity 

        // }).
        // success(function (data, status, headers, config) {
    
             
//               $rootScope.manage_evidences_world_evidences = data;
        //         $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
        //                  placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

        //         $scope.close_modal(my_modal);
           
        // }).
        // error(function (data, status, headers, config) {
        //     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
        //                  placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        // });
    }
});


app.controller('add_new_primary_controller', function($scope, $alert,$http,$rootScope,request_years_from_curri_choosen_service){
 
    $scope.my_new_evidence = {};
   $scope.my_new_evidence.evidence_real_code = "";
   $scope.my_new_evidence.evidence_name = "";
   $scope.my_new_evidence.secret = false;
   $scope.my_new_evidence_file = [];
 $scope.my_temp_secret_new = false;
  $scope.my_new_evidence.teacher_id = "00007";
$scope.primary_choosen = {}

    $scope.init =function() {
         $scope.my_temp_secret_new = false;
   $scope.my_new_evidence = {};
   $scope.my_new_evidence.evidence_real_code = "";
   $scope.my_new_evidence.evidence_name = "";
   $scope.my_new_evidence_file = [];
     $scope.my_new_evidence.secret = false;
      $scope.my_new_evidence.evidence_name = "";
        $scope.my_new_evidence.teacher_id = "00007";
        $scope.primary_choosen = {}
}

    $scope.$on("fileSelected", function (event, args) {
        $scope.$apply(function () {            
            $scope.my_new_evidence_file = [];
            //add the file object to the scope's files collection
            $scope.my_new_evidence_file.push(args.file);
            console.log("fileSelected");
            console.log($scope.my_new_evidence_file);
        });
    });



    $scope.new_evidence_still_not_complete =  function(){
        if (angular.isUndefined($scope.my_new_evidence.primary_choosen) == true ||angular.isUndefined($scope.my_new_evidence.evidence_real_code) == true || angular.isUndefined($scope.my_new_evidence.evidence_name) == true || $scope.my_new_evidence_file.length ==0){

                return true;
        }
        else{
             if ($scope.my_new_evidence.evidence_real_code <= 0){
                   return true;
                }
            return false;
        }
    }

 $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.save_to_server =function(){
        if($scope.my_temp_secret_new  == false){
            $scope.my_new_evidence.secret = 0;
        }
        else{
            $scope.my_new_evidence.secret = 1;
        }



      var formData = new FormData();
$scope.my_new_evidence.file_name = $scope.my_new_evidence_file[0].name;
$scope.my_new_evidence.curri_id =   $rootScope.manage_evidence_curri_id_now;
$scope.my_new_evidence.aca_year = $rootScope.manage_evidence_year_now;



    
    formData.append("model", angular.toJson($scope.my_new_evidence));
    formData.append("file" , $scope.my_new_evidence_file[0]);
             console.log("save to sserver");
console.log($scope.my_new_evidence);

        // $http({
        //     method: 'PUT',
        //     url: "/Api/aunbook",

        //     headers: { 'Content-Type': undefined },


        //     data:formData,
        //     transformRequest: angular.indentity 

        // }).
        // success(function (data, status, headers, config) {
    
             
//               $rootScope.manage_evidences_world_evidences = data;
        //         $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
        //                  placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

        //         $scope.close_modal(my_modal);
           
        // }).
        // error(function (data, status, headers, config) {
        //     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
        //                  placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        // });
    }
});

app.controller('manage_evidences_controller', function($scope, $alert,$http,$rootScope,request_years_from_curri_choosen_service){

        $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                $scope.result={};
                $scope.my_president ={};
                $scope.personnel_choose = {};

   $scope.remove_evidence = function(index_evidence_to_remove) { 
      $rootScope.manage_evidences_world_evidences.splice(index_evidence_to_remove, 1);     

    }

    $scope.watch_file = function(path) { 
        window.open(path, '_blank', "width=800, left=230,top=0,height=700");  
    }

$scope.choose_to_change_file = function(this_obj){
    $rootScope.only_object_want_to_change = angular.copy(this_obj);
    console.log("manage_evidence_indicator_num");
    console.log($rootScope.manage_evidence_indicator_num);
    $rootScope.manage_evidence_indicator_num = $scope.indicator_choosen.indicator_num;
 if($rootScope.only_object_want_to_change.secret == 1){
    $rootScope.only_object_want_to_change.secret = true;
   }
   else{
     $rootScope.only_object_want_to_change.secret = false;
   }




}


$scope.choose_to_add_new_file = function(){
   
    console.log($rootScope.manage_evidence_indicator_num);
    $rootScope.manage_evidence_indicator_num = $scope.indicator_choosen.indicator_num;
    $rootScope.manage_evidence_curri_id_now = $scope.curri_choosen.curri_id;
    $rootScope.manage_evidence_year_now = $scope.year_choosen.aca_year;
}


$scope.choose_to_add_new_primary_file = function(){
   
    console.log($rootScope.manage_evidence_indicator_num);
    $rootScope.manage_evidence_indicator_num = $scope.indicator_choosen.indicator_num;
    $rootScope.manage_evidence_curri_id_now = $scope.curri_choosen.curri_id;
    $rootScope.manage_evidence_year_now = $scope.year_choosen.aca_year;
}
$scope.init =function() {
     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                $scope.result={};
                $scope.my_president ={};
                       $scope.personnel_choose = {};
}


    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    
       $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen).then(function(data) {
            console.log($scope.corresponding_aca_years);
            $scope.corresponding_aca_years = data;
            // $scope.corresponding_aca_years = [2551,2555,2558,2559];
          });


    }
   $scope.find_information = function(){

    $scope.indicator_choosen.curri_id = $scope.curri_choosen.curri_id;
       $http.post(
             '/api/evidence/getwithtname',
             JSON.stringify($scope.indicator_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
              $rootScope.manage_evidences_world_evidences = data;
            $scope.choose_not_complete =false;

         });


   }

      $scope.find_indicators = function(){
$scope.choose_not_complete =true;
          console.log("find_indicators");
        console.log($scope.year_choosen);
        $scope.indicator_choosen = {};
        $http.post(
             '/api/indicator/querybycurriculumacademic',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
              $scope.corresponding_indicators = data;
            // $scope.get_all_teachers();

         });

    }


});

app.controller('manage_sub_indicators_controller', function($scope, $alert,$http,$rootScope){

    // $scope.init = function(){
    //     console.log("manage_sub_indicators_controller init")
    //         $scope.save_indicator = $rootScope.manage_indicators_indicator_choosen;
    // }

    // $scope.$on("open_sub_indicator_modal", function (event, args) {
    //     $scope.$apply(function () {            
    //         //add the file object to the scope's files collection
    //         console.log("backup working...")

    //         $scope.save_indicator = $rootScope.manage_indicators_indicator_choosen;
    //     });
    // });


             $scope.still_not_choose_complete_sub =function(){



// var index;
// if(angular.isUndefined($rootScope.manage_indicators_indicator_choosen.sub_indicator_list)){
//     $rootScope.manage_indicators_indicator_choosen.sub_indicator_list = [];
// }

// $scope.my_sub_num_indicators = [];
// for (index =0;index<  $rootScope.manage_indicators_indicator_choosen.sub_indicator_list.length ; index++){

//      if($scope.my_sub_num_indicators.indexOf($rootScope.manage_indicators_indicator_choosen.sub_indicator_list[index].sub_indicator_num) == -1) {
//         $scope.my_sub_num_indicators.push($rootScope.manage_indicators_indicator_choosen.sub_indicator_list[index].sub_indicator_num);
//     }
//     else {
    
//        return true;

//     }

//      if( $rootScope.manage_indicators_indicator_choosen.sub_indicator_list[index].sub_indicator_name == "" || $rootScope.manage_indicators_indicator_choosen.sub_indicator_list[index].sub_indicator_num == ""){
//           return true;

//               }

// }
          

     

//         return false;
      }


    $scope.recover_indicator = function(){
        // console.log("backup is");
        // console.log($rootScope.manage_indicators_and_sub_save_indicator);
        // console.log("to replace");
        // console.log($rootScope.manage_indicators_indicator_choosen);

        // angular.copy($rootScope.manage_indicators_and_sub_save_indicator,$rootScope.manage_indicators_indicator_choosen);
       $rootScope.manage_indicators_and_sub_result[$rootScope.manage_indicators_and_sub_save_indicator.save_index] = {};
     
       $rootScope.manage_indicators_indicator_choosen = {};

       console.log("index");
       console.log($rootScope.manage_indicators_and_sub_save_indicator.save_index);
console.log("backup");
console.log($rootScope.manage_indicators_and_sub_save_indicator.save_content);
       $rootScope.manage_indicators_indicator_choosen = angular.copy($rootScope.manage_indicators_and_sub_save_indicator.save_content);
         $rootScope.manage_indicators_and_sub_result[$rootScope.manage_indicators_and_sub_save_indicator.save_index] =  angular.copy($rootScope.manage_indicators_indicator_choosen);
    

    console.log("manage_indicators_indicator_choosen");
console.log($rootScope.manage_indicators_indicator_choosen);
    console.log("all");
    console.log($rootScope.manage_indicators_and_sub_result);

    }

        $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
     $scope.add_sub_indicator = function(){


         $rootScope.manage_indicators_indicator_choosen.sub_indicator_list.push({
            "aca_year":$rootScope.manage_indicators_and_subs_year_choosen
            ,"indicator_num":$rootScope.manage_indicators_indicator_choosen.indicator_num,
            "sub_indicator_num":"","sub_indicator_name":""});
      }


      $scope.remove_sub_indicator = function(index_sub_indicator_to_remove) { 
      $rootScope.manage_indicators_indicator_choosen.sub_indicator_list.splice(index_sub_indicator_to_remove, 1);     
      console.log($rootScope.manage_indicators_indicator_choosen);
    }

        $scope.close_modal = function(my_modal){
       
        my_modal.$hide();
    }


    $scope.save_to_server = function(my_modal){

        $rootScope.manage_indicators_indicator_choosen.aca_year = $rootScope.manage_indicators_year_to_create;
        console.log("save_to_server");
        console.log($rootScope.manage_indicators_indicator_choosen);

        if($rootScope.manage_indicators_indicator_choosen.sub_indicator_list.length==0){
              $rootScope.manage_indicators_indicator_choosen.sub_indicator_list.push({
            "aca_year":$rootScope.manage_indicators_and_subs_year_choosen
            ,"indicator_num":$rootScope.manage_indicators_indicator_choosen.indicator_num,
            "sub_indicator_num":"","sub_indicator_name":""});
        }
        $http.put(
             '/api/indicatorsubIndicator/savesubindicator',
             JSON.stringify($rootScope.manage_indicators_indicator_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {


               $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
                $rootScope.manage_indicators_and_sub_result[$rootScope.manage_indicators_and_sub_save_indicator.save_index] = angular.copy($rootScope.manage_indicators_indicator_choosen);
                $rootScope.my_backup_indicators[$rootScope.manage_indicators_and_sub_save_indicator.save_index] = angular.copy($rootScope.manage_indicators_indicator_choosen);
                $rootScope.manage_indicators_and_sub_save_indicator.save_content = angular.copy($rootScope.manage_indicators_indicator_choosen);
                $scope.close_modal(my_modal);

         })
    .error(function(data, status, headers, config) {
                  if(status==500){

     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
    }
});

app.controller('manage_primary_evidences_admin_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {

      $http.get('/api/curriculumacademic/getdistinctacayear').success(function (data) {
            
           $scope.corresponding_aca_years =data;
          });
// $scope.corresponding_aca_years = [2551,2553,2555,2558];
 $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {};
               $scope.indicator_choosen= {};
     
                 $scope.corresponding_indicators = {};
               
$scope.init =function() {
     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {};
               $scope.indicator_choosen= {};

                 $scope.corresponding_indicators = {};
}

      $scope.still_not_choose_complete =function(){


if($scope.choose_not_complete==false){
var index;
for (index =0;index< $scope.result.length ; index++){
     if($scope.result[index].evidence_name == "" ){
          return true;

              }

}
          

       }

        return false;
      }




$scope.add_primary_evidence = function(){
    console.log($scope.year_choosen.aca_year);

         $scope.result.push({ "evidence_name":"","just_create":true,"curri_id":"0","aca_year":$scope.year_choosen,"indicator_num":$scope.indicator_choosen.indicator_num
});
      }

    $scope.remove_primary_evidence = function(index_primary_evidence_to_remove) { 
      $scope.result.splice(index_primary_evidence_to_remove, 1);     

    }
  $scope.find_indicators = function(){

          console.log("find_indicators");
        console.log($scope.year_choosen);
$scope.indicator_choosen = {};
        $http.post(
             '/api/indicator/querybyacademicyear',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
              $scope.corresponding_indicators = data;
               $scope.choose_not_complete = true;

         });

    }

  $scope.find_primary_evidences = function(){

          console.log("find_primary_evidences");


        $http.post(
             '/api/primaryevidence/adminget',
             JSON.stringify($scope.indicator_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
              $scope.result = data;
            $scope.choose_not_complete = false;


         });

    

    }

    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
      $scope.save_to_server = function(my_modal){
        console.log("save_to_server");
        console.log($scope.result);

        if ($scope.result.length == 0){
             $scope.result.push({'primary_evidence_num':-1,'aca_year':$scope.year_choosen,"curri_id":"0",'indicator_num':$scope.indicator_choosen.indicator_num,"evidence_name":""});
        }
        $http.put(
             '/api/primaryevidence/adminsave',
             JSON.stringify($scope.result),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
               $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
               $scope.close_modal(my_modal);
         })
    .error(function(data, status, headers, config) {
                  if(status==500){

     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
    }


});



app.controller('manage_primary_evidences_president_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {};
               $scope.indicator_choosen= {};
                $scope.corresponding_aca_years = {};
                 $scope.corresponding_indicators = {};
}
     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {};
               $scope.indicator_choosen= {};
 $scope.corresponding_aca_years = {};
       $scope.corresponding_indicators = {};

$scope.all_teachers = [{"teacher_id":"00001","degree":"1","position":"1","personnel_type":"1","person_id":"1","status":"normal","alive":49,"is_admin":"1","username":"kpwiboon","password":"1111111","user_type":"teacher","t_prename":"mr.","t_name":"wiboonth","e_prename":"mr.","e_name":"wiboon","citizen_id":"12448","gender":"m","email":"kpwiboon","tel":"000000","addr":"sd","file_name_pic":"ssd","timestamp":"2016-01-22T00:00:00+07:00"},{"teacher_id":"00002","degree":"2","position":"1","personnel_type":"1","person_id":"1","status":"normal","alive":50,"is_admin":"0","username":"kwakaarad","password":"10111","user_type":"teacher","t_prename":"mr.","t_name":"akaaradth","e_prename":"mr.","e_name":"akaara","citizen_id":"1248","gender":"m","email":"kwakkarad","tel":"000000","addr":"sd","file_name_pic":"ssd","timestamp":"2016-01-22T00:00:00+07:00"}];

// $scope.find_corresponding_teacher_obj = function(teacher_id_in){
//          angular.forEach($scope.all_teachers, function(value, key) {
                 
//                   if($scope.all_teachers[key].teacher_id == teacher_id_in){
//                     console.log("found");
//                     console.log($scope.all_teachers[key]);

//                     var obj = 
//                     return {
//                         teacher_id:  teacher_id_in,
//                         t_name: $scope.all_teachers[key].t_name
//                     };
//                   }
//                 });
// }
$scope.just_show_responsible = function(ask_status){
    if(ask_status == "5" || ask_status =="1"){
        return true;
    }
    else{
        return false;
    }
}
$scope.name_of_teacher_id = function(ask_id){
    var index;
    for(index=0;index<$scope.all_teachers.length;index++){
        if($scope.all_teachers[index].teacher_id == ask_id){
            return $scope.all_teachers[index].t_name;
        }
    }
}
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen).then(function(data) {

            $scope.corresponding_aca_years = data;
            // $scope.corresponding_aca_years = [2551,2555,2558,2559];
          });


    }

     $scope.add_primary_evidence = function(){
        // $scope.new_indicator = {"sub_indicator_list":[]
        // ,"aca_year":$scope.year_choosen
        // ,"indicator_num":$rootScope.manage_indicators_and_sub_result.length+1
        // ,"indicator_name_t":"","indicator_name_e":""}


         $scope.result.push({ "primary_evidence_num":1,
"aca_year":$scope.year_choosen.aca_year ,
"indicator_num":$scope.indicator_choosen.indicator_num ,
"curri_id":$scope.curri_choosen.curri_id ,
"evidence_name":"",
"just_create":true,
"teacher_id":"",
"status":"2"});
      }

$scope.choose_teacher = function(my_obj){
    if(my_obj.status == "2"){
        my_obj.by_pass="1";
    }
}

      $scope.still_not_choose_complete =function(){
            // angular.forEach($scope.result, function(value, key) {
                 
                  
            //      if($scope.result[key].evidence_name == "" || ($scope.result[key].teacher_id == "" && $scope.result[key].just_create == true)){
            //         console.log("true");
            //         return 0;
            //      }
            //     else{
            //         if (key== $scope.result.length-1){
            //              return 1;
            //         }
            //     }
               
            //     });

if($scope.choose_not_complete==false){
var index;
for (index =0;index< $scope.result.length ; index++){
     if($scope.result[index].evidence_name == "" || $scope.result[index].teacher_id == "" ){
          return true;

              }

}
          

       }

        return false;
      }


$scope.send_email = function(teacher_id_to_send){

if(angular.isUndefined(teacher_id_to_send)){
      $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาเลือกผู้รับผิดชอบหลักฐานก่อนส่ง',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});

}else{
           $http.post(
             '/api/primaryevidence/sendmail',
             JSON.stringify(teacher_id_to_send),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
    
                $alert({title:'ดำเนินการสำเร็จ', content:'ส่ง Email แจ้งเตือนเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

         })
    .error(function(data, status, headers, config) {
                  if(status==500){
     $alert({title:'เกิดข้อผิดพลาด', content:'ส่ง Email แจ้งเตือนไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
}



}
      $scope.remove_primary_evidence = function(index_primary_evidence_to_remove) { 
        if($scope.result[index_primary_evidence_to_remove].status == "2"){
                $scope.result.splice(index_primary_evidence_to_remove, 1);   
        }
        else{
            if($scope.result[index_primary_evidence_to_remove].status == "0" || $scope.result[index_primary_evidence_to_remove].status == "1"){
                $scope.result[index_primary_evidence_to_remove].status = "3";
            }
            else  if($scope.result[index_primary_evidence_to_remove].status == "4" || $scope.result[index_primary_evidence_to_remove].status == "5"  || $scope.result[index_primary_evidence_to_remove].status == "6"){
                $scope.result[index_primary_evidence_to_remove].status = "7";
            }
        }
    

    }
 $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

  $scope.find_indicators = function(){

          console.log("find_indicators");
        console.log($scope.year_choosen);
$scope.indicator_choosen = {};
        $http.post(
             '/api/indicator/querybycurriculumacademic',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
              $scope.corresponding_indicators = data;
            $scope.get_all_teachers();

         });

    }

    $scope.get_all_teachers = function(){

      

                $http.post(
             '/api/teacher/getname',
             JSON.stringify($scope.curri_choosen.curri_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
               $scope.all_teachers = data;
               

         });
    }
    $scope.find_information = function(){
$scope.indicator_choosen.curri_id = $scope.curri_choosen.curri_id;
 
        $http.post(
             '/api/primaryevidence/presidentcurriget',
             JSON.stringify($scope.indicator_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            console.log("this is data we received")
            console.log(data);
             $scope.result = data;
             $scope.choose_not_complete = false;
         });

 // $scope.result =[


 // {"teacher_id":"00001","status":"0","primary_evidence_num":1,"aca_year":2558,"indicator_num":1,"evidence_name":"หลักฐาน1","curri_id":"21"},
 // {"teacher_id":"00001","status":"1","primary_evidence_num":2,"aca_year":2558,"indicator_num":1,"evidence_name":"หลักฐาน2","curri_id":"21"},
 // {"teacher_id":"00001","status":"0","primary_evidence_num":3,"aca_year":2558,"indicator_num":1,"evidence_name":"หลักฐาน3","curri_id":"21"},
 // {"teacher_id":"","status":"6","primary_evidence_num":4,"aca_year":2558,"indicator_num":1,"evidence_name":"หลักฐาน4","curri_id":"21"},
 // {"teacher_id":"","status":"6","primary_evidence_num":4,"aca_year":2558,"indicator_num":1,"evidence_name":"หลักฐาน5","curri_id":"21"},
 // {"teacher_id":"00001","status":"4","primary_evidence_num":5,"aca_year":2558,"indicator_num":1,"evidence_name":"หลักฐาน6","curri_id":"21"},
 // {"teacher_id":"00001","status":"5","primary_evidence_num":6,"aca_year":2558,"indicator_num":1,"evidence_name":"หลักฐาน7","curri_id":"21"}]
 // $scope.choose_not_complete = false;

    }
$scope.dont_show_me =function(my_obj){
    if(my_obj.status == '3' || my_obj.status == '7' ){
        return true;
    }
    else{
        return false;
    }
}
 $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

 $scope.save_to_server = function(my_modal){

    
        $http.put(
             '/api/primaryevidence/presidentcurrisave',
             JSON.stringify($scope.result),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {


               $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
              
                $scope.close_modal(my_modal);

         })
    .error(function(data, status, headers, config) {
                  if(status==500){

     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
    }







});







app.controller('manage_survey_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
}
  
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
  
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen).then(function(data) {

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
            
              $scope.result = data;
             $scope.choose_not_complete = false;
             
            
    
         });

    }
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
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
               $scope.close_modal(my_modal);
         })
    .error(function(data, status, headers, config) {
                  if(status==500){

     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
    }
});


app.controller('import_evidence_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
 $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {};
                $scope.evidence_we_want = {};
                    $scope.result = {};


   $scope.watch_file = function(path) { 
        window.open(path, '_blank', "width=800, left=230,top=0,height=700");  
    }


   $scope.choose_evidence_already = function(){
    $scope.choose_not_complete = false;
   }
$scope.watch_preview = function(){
    if($scope.choose_not_complete == false){
        $scope.watch_file($scope.evidence_we_want.file_name);
    }
    else{
         $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาเลือกหลักฐานที่ต้องการดู',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
    }
}

$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.evidence_we_want = {}
                    $scope.result = {};
                }
  
  
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
     
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen).then(function(data) {

            $scope.corresponding_aca_years = data;

          });


    }

    $scope.find_all_evidences_in_curri_and_year = function(){

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
            
              $scope.all_evidences = data;
             $scope.choose_not_complete = true;
       
   
         });

    }
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
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
               $scope.close_modal(my_modal);
         })
    .error(function(data, status, headers, config) {
                  if(status==500){

     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
    }
});


