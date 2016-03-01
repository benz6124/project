'use strict';



app.controller('choice_index_controller', function($scope, $http,$alert,$cookies,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope) {


 
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
     $scope.not_choose_year = true;
       $rootScope.all_curriculums = [];

    // console.log(select_nothing);


    $rootScope.clear_choosen = function(){
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
         $scope.not_choose_year = true;
         $scope.corresponding_aca_years = [];
    }
    $scope.can_edit_reason = function(){

    if($scope.$parent.already_login == true){
        if(!$rootScope.current_user.privilege[$scope.curri_choosen.curri_id]){
            return false;
        }
        if( $rootScope.current_user.privilege[$scope.curri_choosen.curri_id]['15'] ==2){
        return true;
       }

       if($rootScope.president_in_this_curri_and_year($scope.curri_choosen.curri_id,$scope.year_choosen.aca_year)==true){
        return true;
       }

       if($rootScope.right_from_committee($scope.curri_choosen.curri_id,$scope.year_choosen.aca_year,15,2)==true){
        return true;
       }
    }
       return false;
    }

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
     $scope.not_choose_year = true;
       $rootScope.all_curriculums = [];


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
        var index;
        $scope.corresponding_aca_years_except_us = [];
        for(index=0;index<$scope.corresponding_aca_years.length;index++){
            if($scope.corresponding_aca_years[index].aca_year != $scope.year_choosen.aca_year){
                $scope.corresponding_aca_years_except_us.push($scope.corresponding_aca_years[index]);
            }
        }
    }
    $scope.choose_overall = function(){
        $scope.select_overall = true;
        $scope.get_all_evaluation();
    }
    $scope.sendCurriAndGetYears = function (curri) {
        $scope.select_all_complete = false;
         $scope.not_select_curri_and_year = true;
         $scope.already_select_curri = true;
           $scope.not_choose_year = true;
           $scope.year_choosen = {};
            $scope.$parent.not_choose_curri_and_year_yet = true;
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
  

    $scope.check_curri = function(){
        // console.log("welcome check_curri");
        if ($scope.already_select_curri == false){
              $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาเลือกหลักสูตรที่ต้องการก่อน',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
                
        }
    }
     $scope.sendYearAndGetIndicators = function (year) {
         $scope.select_all_complete = true;
        if ($scope.select_all_complete  == true){
            
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
                  $scope.not_select_curri_and_year = false;
           $scope.choose_overall();
            $scope.not_select_sub_indicator = true;
                   $scope.not_choose_year = false;

                   $scope.$parent.not_choose_curri_and_year_yet = false;
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
    
     $scope.get_all_evaluation = function (){

         $http.post(
             '/api/evaluationresult/overall',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.evaluation_overall = data;

            var index;
            var inner_index;
            for(index=0;index<$scope.evaluation_overall.length;index++){
                if(angular.isNumber($scope.evaluation_overall[index].indicator_result_self_average)==false || angular.isNumber($scope.evaluation_overall[index].indicator_result_other_average)==false){
                    $scope.evaluation_overall[index].complete_both = false;
                }
                else{
                    $scope.evaluation_overall[index].complete_both = true;
                }

            }

            var index;
var inner_index;
for(index=0;index<$scope.evaluation_overall.length;index++){
    if(angular.isNumber($scope.evaluation_overall[index].indicator_result_self_average)==false || angular.isNumber($scope.evaluation_overall[index].indicator_result_other_average)==false){
        $scope.evaluation_overall[index].complete_both = false;
    }
    else{
        $scope.evaluation_overall[index].complete_both = true;
    }

}

         });


     }

    $scope.sendIndicatorCurriAndGetEvaluation = function () {
        console.log($scope.indicator_choosen)
        $scope.this_indicator_show = {};
        $scope.self_evaluated = true;
        $scope.others_evaluated = true;
        var index;
        for(index=0;index<$scope.evaluation_overall.length;index++){
            if($scope.evaluation_overall[index].indicator_num == $scope.indicator_choosen.indicator_num){
                $scope.this_indicator_show = $scope.evaluation_overall[index];
            }
        }
        if(!$scope.this_indicator_show.indicator_result_self_average){
                   $scope.self_evaluated = false;
        }
        if(!$scope.this_indicator_show.indicator_result_other_average){
               $scope.others_evaluated = false;
        }

        $scope.sendIndicatorCurriAndGetEvidence();
        // // $scope.loading = new $loading();
        // // $scope.loading.show();
        // $scope.indicator_choosen.curri_id = $scope.curri_choosen.curri_id;
        // $scope.evaluation_result_receive
    }

    $scope.download_file = function(path) { 
        window.open(path, '_blank', "");  
    }


    $scope.watch_file = function(this_evidence) { 
    
                window.open(this_evidence.file_name, '_blank', "width=800, left=230,top=0,height=700");  
      
       
      
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
$scope.get_support_content_from_other_year = function (my_modal) {
    CKEDITOR.instances['support_text'].setData($scope.show_preview_support_text);

 my_modal.$hide();
     $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
    // alert( CKEDITOR.instances['support_text'].getData());
    // CKEDITOR.instances['support_text'].setData("cheese pizza");
}


$scope.prepare_to_watch_support_text = function(){
    console.log("prepare_to_watch_support_text");
    $scope.sendSectionSaveAndGetPreviewSupportText();
}

$scope.download_aun_book = function(){

     if ($scope.select_all_complete  == true){

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


         })
         .error(function (data) {
            
                $alert({title:'เกิดข้อผิดพลาด', content:'ไม่สามารถดาวน์โหลดได้',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});


         });

          
    }
    else{
    $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาเลือกหลักสูตรและปีการศึกษาก่อน',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});

         }
}
$scope.send_support_text_change_to_server = function(){
    console.log("send_support_text_change_to_server");


    $scope.current_section_save.detail = CKEDITOR.instances['support_text'].getData();
    $scope.current_section_save.teacher_id = $rootScope.current_user.user_id;
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
        $scope.curri_choosen = {};
               $scope.new_curri_academic = {};
        $scope.new_curri_academic.aca_year = ""
 $scope.please_wait = false;
        $scope.error_leaw = false;
      $scope.all_curri_that_have_privileges = [];
      if($rootScope.current_user.user_type == 'ผู้ดูแลระบบ'){
        $scope.all_curri_that_have_privileges = $rootScope.all_curriculums;
      }else{
         $scope.$parent.scan_only_privilege_curri('2',$scope.all_curri_that_have_privileges);
      }
 
    }

       $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    
  $scope.still_not_complete = function(){
    if(!$scope.new_curri_academic){
        return true;
    }

    if(!  $scope.curri_choosen || !$scope.new_curri_academic.aca_year){
        return true;
    }
    
    if(angular.isNumber($scope.new_curri_academic.aca_year)==false || $scope.new_curri_academic.aca_year <= 0){
           return true;
    }

    return false;
  }

    $scope.curri_choosen = {};
               $scope.new_curri_academic = {};
        $scope.new_curri_academic.aca_year = ""
 
        $scope.error_leaw = false;
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.add_aca_year_to_server = function(my_modal){
         $scope.please_wait = true;
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
 $scope.please_wait = false;
                  if(status==400){

     $alert({title:'เกิดข้อผิดพลาด', content:data.message,alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 

     }else{
         $scope.please_wait = false;
       $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาเลือกหลักสูตรและปีการศึกษา',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});

     }
    }
});
app.controller('create_curriculum_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope) {
    $scope.init = function(){
        $scope.new_curri = {}
 $scope.please_wait = false;
  $scope.new_curri.level = {};

    }

       $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    

 $scope.new_curri = {}
  $scope.new_curri.level = {};
     $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    

    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

$scope.still_not_complete = function(){
    if(!$scope.new_curri){
        return true;
    }
 if (!$scope.new_curri.curr_tname || !$scope.new_curri.curr_ename || !$scope.new_curri.degree_t_full ||  !$scope.new_curri.degree_t_bf || !$scope.new_curri.degree_e_full||  !$scope.new_curri.degree_e_bf ||  !$scope.new_curri.level || !$scope.new_curri.period ){
    return true;
 }

 if($scope.new_curri.level != 1 && $scope.new_curri.level != 2 && $scope.new_curri.level != 3 ){
    return true;
 }

 return false;
}

    $scope.create_curri = function(my_modal){
          console.log($scope.new_curri);
 $scope.please_wait = true;
       
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
                 $rootScope.clear_choosen();
                   $alert({title:'ดำเนินการสำเร็จ', content:'สร้างหลักสูตรเรียบร้อยแล้ว',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
                   my_modal.$hide();
                    $scope.new_curri = {};

         //เรียกฟังชั่นใน servce ให้อัพเดทค่า


         });
    
    // else{
    //      $scope.please_wait = false;
    //       $alert({title:'เกิดข้อผิดพลาด', content:'กรุณากรอกข้อมูลให้ครบถ้วน',alertType:'danger',
    //                      placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
    // }
}
});


app.controller('stat_graduated_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                     $scope.all_curri_that_have_privileges = [];
                           $scope.corresponding_aca_years = [];
  $scope.$parent.scan_only_privilege_curri('11',$scope.all_curri_that_have_privileges);

}
  
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen,11,2).then(function(data) {
$scope.corresponding_indicators = [];
            $scope.corresponding_aca_years = data;
   $scope.choose_not_complete = true;
          });


    }

     $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    

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
            console.log("interr")
                console.log(data)
              $scope.result = data;
             $scope.choose_not_complete = false;
              $scope.blank = false;
              var value;
              var key;
             if ($scope.result.grad_in_time==-1){
                console.log("-1 ka")
               
                angular.forEach($scope.result, function(value, key) {
                 
                  
                  if(key !="year" && key != "curri_id"){
                   
                    $scope.result[key] = "";
                    $scope.blank = true;
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
               $scope.close_modal(my_modal);
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



app.controller('stat_student_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {}
               $scope.indicator_choosen= {};
                  $scope.result ={};
                   $scope.all_curri_that_have_privileges = [];
                         $scope.corresponding_aca_years = [];
  $scope.$parent.scan_only_privilege_curri('13',$scope.all_curri_that_have_privileges);
}

 $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });


   
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen,13,2).then(function(data) {
$scope.corresponding_indicators = [];
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
            console.log(data)
               $scope.result = data;
             $scope.choose_not_complete = false;
             
              var value;
              var key;
              $scope.blank = false;
             if ($scope.result.ny1==-1){
                
                angular.forEach($scope.result, function(value, key) {
                 
                  
                  if(key !="year" && key != "curri_id"){
                   
                    $scope.result[key] = "";
                      $scope.blank = true;
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
                 $scope.close_modal(my_modal);
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



app.controller('stat_new_student_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
       $scope.year_choosen = {};
              $scope.curri_choosen = {};
                 $scope.indicator_choosen= {};
                   $scope.result = {};
                    $scope.all_curri_that_have_privileges = [];
                          $scope.corresponding_aca_years = [];
  $scope.$parent.scan_only_privilege_curri('12',$scope.all_curri_that_have_privileges);

}
    
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen,12,2).then(function(data) {
$scope.corresponding_indicators = [];
            $scope.corresponding_aca_years = data;

          });


    }


     $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });


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
                  $scope.blank = false;
             if ($scope.result.num_admis_f==-1){
            
                angular.forEach($scope.result, function(value, key) {
                 
                  
                  if(key !="year" && key != "curri_id"){
                   
                    $scope.result[key] = "";
                    $scope.blank = true;
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
                  $scope.close_modal(my_modal);
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

app.controller('evaluate_by_me_controller', function($scope, $alert,$http,request_years_from_curri_choosen_service,$rootScope) {
$scope.init =function() {
     $scope.choose_not_complete = true;
   $scope.year_choosen = {};
       $scope.curri_choosen = {};
$scope.indicator_choosen = {};

            
                $scope.corresponding_aca_years = [];
                 $scope.corresponding_indicators = [];
 $scope.all_curri_that_have_privileges = [];
  $scope.$parent.scan_only_privilege_curri('4',$scope.all_curri_that_have_privileges);
}

   $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });


       $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }


$scope.still_not_complete = function(){
    var index;
    if(!$scope.corresponding_results){
        return true;
    }

    for(index=0;index<$scope.corresponding_results.length;index++){
        if (!$scope.corresponding_results[index].evaluation_score){
            return true;
        }
    }

    return false;
   }

      $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen,4,2).then(function(data) {
$scope.corresponding_indicators = [];
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

        var index;
        for(index=0;index<$scope.corresponding_results.length;index++){
            $scope.corresponding_results[index].teacher_id = $rootScope.current_user.user_id;
        }

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


app.controller('evaluate_by_other_controller', function($scope,$rootScope, $alert,$http,request_years_from_curri_choosen_service) {
    $scope.init =function() {
 
     $scope.choose_not_complete = true;
           $scope.year_choosen = {};
              $scope.curri_choosen = {}
  $scope.files = [];
     $scope.year_choosen = {};
       $scope.please_wait = false;
$scope.indicator_choosen = {};
 $scope.corresponding_aca_years = [];
                 $scope.corresponding_indicators = [];
  $scope.disabled_search = false;
  $scope.all_curri_that_have_privileges = [];
       var index;
    var index2;

for(index=0;index<$rootScope.all_curriculums.length;index++){
    for(index2 = 0;index2<$rootScope.current_user.curri_id_in.length ;index2++){
      
        if($rootScope.all_curriculums[index].curri_id ==$rootScope.current_user.curri_id_in[index2] ){
             $scope.all_curri_that_have_privileges.push($rootScope.all_curriculums[index]);
        }
    }
}

   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
}


  $scope.disabled_search = false;

   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });


      $scope.set_disabled_search = function(){
        console.log("disabled_search");
        console.log($scope.disabled_search);
        $scope.disabled_search = true;
    }

    $scope.watch_file = function() { 
    
                window.open($scope.corresponding_results.file_name, '_blank', "width=800, left=230,top=0,height=700");  
      
       
      
    }

     $scope.back_to_default = function(){

        if(!$scope.corresponding_results.file_name){
                    angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });

            $scope.files = [];
        }
        else{

               $scope.disabled_search = false;
            angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });

            $scope.files = [];
        }
      
     }
   $scope.still_not_complete = function(){
    var index;
    if(!$scope.corresponding_results){
        return true;
    }
    for(index=0;index<$scope.corresponding_results.length;index++){
        if (!$scope.corresponding_results[index].evaluation_score){
            return true;
        }
    }

    return false;
   }

    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {


    

              $scope.init();


    });

  $scope.$on("fileSelected", function (event, args) {
        $scope.$apply(function () {            
           var extension = args.file.name.split('.');

 if(args.file.size > 25000000){
                   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                $alert({title:'เกิดข้อผิดพลาด', content:'ไฟล์ที่เลือกมีขนาดมากกว่า 25 MB',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
            else        if(extension[extension.length-1] == 'exe' || extension[extension.length-1] == 'EXE' || extension[extension.length-1] == 'vb' || extension[extension.length-1] == 'VB'
        || extension[extension.length-1] == 'bat' || extension[extension.length-1] == 'BAT'  || extension[extension.length-1] == 'ini' || extension[extension.length-1] == 'INIT' ){
        
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                 
         $alert({title:'เกิดข้อผิดพลาด', content:'ไม่อนุญาติให้อัพโหลดไฟล์นามสกุลดังกล่าว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }
            else{
                $scope.files = [];
                   $scope.files.push(args.file);
            }

         
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
             $scope.choose_not_complete = true;
        $scope.corresponding_indicators = [];
                    $scope.please_wait = false;
          });



    }

$scope.clear_files = function(){
    $scope.files = [];
}
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
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
      $scope.choose_not_complete = true;
             $scope.corresponding_indicators = data;
                    $scope.please_wait = false;
         });

    }

    $scope.get_results= function(){
        console.log($scope.indicator_choosen);
        $scope.indicator_choosen.curri_id = $scope.curri_choosen.curri_id ;
          $scope.indicator_choosen.aca_year = $scope.year_choosen.aca_year ;
        $http.post(
             '/api/othersevaluation',
             JSON.stringify($scope.indicator_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
                   console.log("wait results");
            console.log(data);
        $scope.please_wait = false;
             $scope.corresponding_results = data;
             $scope.choose_not_complete = false;

                        if(!$scope.corresponding_results.file_name){
            console.log("start disabled_search = true")
              $scope.disabled_search = true;
        }
     
         });

    }


    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

 $scope.save_to_server = function(my_modal) {

          $scope.please_wait = true;
      var formData = new FormData();
$scope.to_sent = {};

   

// if( !$scope.files[0]){
//     $scope.to_sent.evaluation_detail = $scope.corresponding_results;
//     $scope.to_sent.file_name = "";
// }else{
//      $scope.to_sent.evaluation_detail = $scope.corresponding_results;
//     $scope.to_sent.file_name = $scope.files[0].name;
// }


if( $scope.files.length != 0){
      $scope.to_sent.file_name = $scope.files[0].name;
}


 formData.append("model", angular.toJson($scope.corresponding_results));
  formData.append("file", $scope.files[0]);
  console.log("$scope.to_sent");
  console.log($scope.corresponding_results);

        $http({
            method: 'PUT',
            url: "/api/othersevaluation",

            headers: { 'Content-Type': undefined },


            data:formData,
            transformRequest: angular.indentity 

        }).
        success(function (data, status, headers, config) {
              $scope.init ();
                   my_modal.$hide();
              $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
         
                
        }).
        error(function (data, status, headers, config) {
              $scope.please_wait = false;
               $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
}


});

app.controller('upload_aun_controller', function($scope, $alert,$http,request_years_from_curri_choosen_service,$rootScope) {

    $scope.init =function() {
        console.log("init");
     $scope.choose_not_complete = true;
           $scope.year_choosen = {};
              $scope.curri_choosen = {}
                    $scope.corresponding_aca_years = [];
  $scope.files = [];
   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
  $scope.please_wait = false;




$scope.all_curri_that_have_privileges = [];
$scope.$parent.scan_only_privilege_curri('14',$scope.all_curri_that_have_privileges);

}
  $scope.please_wait = false;



    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

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
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen,14,2).then(function(data) {
$scope.corresponding_indicators = [];
            $scope.corresponding_aca_years = data;

          });


    }


    //a simple model to bind to and send to the server




    //listen for the file selected event
   $scope.$on("fileSelected", function (event, args) {

     
        var extension = args.file.name.split('.');

   

        console.log(args.file);
        $scope.$apply(function () {    
                $scope.files =[];    
            //add the file object to the scope's files collection
            if(args.file.size > 25000000){
                   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                 $alert({title:'เกิดข้อผิดพลาด', content:'ไฟล์ที่เลือกมีขนาดมากกว่า 25 MB',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
            else        if(extension[extension.length-1] == 'exe' || extension[extension.length-1] == 'EXE' || extension[extension.length-1] == 'vb' || extension[extension.length-1] == 'VB'
        || extension[extension.length-1] == 'bat' || extension[extension.length-1] == 'BAT'  || extension[extension.length-1] == 'ini' || extension[extension.length-1] == 'INIT' ){
        
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });

         $alert({title:'เกิดข้อผิดพลาด', content:'ไม่อนุญาติให้อัพโหลดไฟล์นามสกุลดังกล่าว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }
            else{
                     $scope.files.push(args.file);
            }
       
        });
    });
    

        $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }


    $scope.save_to_server = function(my_modal) {
   $scope.please_wait = true;
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
                      $scope.please_wait = false;
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
                       $scope.current_president = {};
                       $scope.blank_please= false;
                         $scope.corresponding_aca_years =[];
}

  $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });


        $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                $scope.results={};
                $scope.my_president ={};
                $scope.personnel_choose = {};
                $scope.current_president= {};
  $scope.blank_please= false;

  $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen).then(function(data) {
$scope.corresponding_indicators = [];
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
                     console.log('hello it me')
              $scope.results = data;
            $scope.choose_not_complete = false;
                     $scope.blank_please= false;
            if(!$scope.results[0].username){
          
                $scope.results.splice(0,1);
                 $scope.personnel_choose = [];
            $scope.current_president = [];
 $scope.blank_please= true;
            }
            else{

            $scope.personnel_choose = $scope.results[0];
            $scope.current_president = $scope.results[0];
            }



            
         });


    }
$scope.change_already = function(){

    $scope.blank_please= false;
}
    $scope.still_same_president = function(){

        console.log("still_same_president");
        console.log($scope.personnel_choose);
        console.log($scope.current_president);
        if($scope.personnel_choose.teacher_id == $scope.current_president.teacher_id){
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
 



app.controller('manage_admin_add_admin_controller', function($scope, $rootScope,$alert,$http,request_years_from_curri_choosen_service) {
$scope.init =function() {
    $scope.new_admin = {};
    $scope.new_admin.t_name = "";
    $scope.new_admin.email = "";
}
 $scope.new_admin = {};
    $scope.new_admin.t_name = "";
    $scope.new_admin.email = "";


    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

      $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.still_not_complete = function(){
        if (!$scope.new_admin){
            return true;
        }

            if (!$scope.new_admin.t_name || !$scope.new_admin.email){
            return true;
        }

        return false;
    }

       $scope.save_to_server = function(my_modal){

               
    $scope.to_sent = {};
    $scope.to_sent.t_name = $scope.new_admin.t_name;
    $scope.to_sent.email = $scope.new_admin.email;
    $scope.to_sent.user_id = $rootScope.current_user.user_id;
        $http.post(
             '/api/admin',
             JSON.stringify(  $scope.to_sent),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {


            console.log(data);
           $http.get('/api/admin').success(function (data) {
  
             $rootScope.all_admins = data;
           
           });

             $alert({title:'ดำเนินการสำเร็จ', content:'เพิ่มข้อมูลเรียบร้อย ตรวจสอบอีเมล์เพื่อรับรหัสผ่าน',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

               my_modal.$hide();

        
    })
             .error(function(data, status, headers, config) {
                  if(status==500){

     $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 

    }

});

app.controller('manage_admin_who_controller', function($scope, $rootScope,$alert,$http,request_years_from_curri_choosen_service,Lightbox) {
$scope.init =function() {
     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                $scope.results={};
                $scope.my_president ={};
                       $scope.personnel_choose = {};
                       $scope.current_president = {};
                  
                       $scope.email_new_admin = "";
                       $scope.add_admin_mode = false;




   $http.get('/api/admin').success(function (data) {
  
             $rootScope.all_admins = data;
           
           });


}
        $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                $scope.results={};
                $scope.my_president ={};
                $scope.personnel_choose = {};
                $scope.current_president= {};
         
   $scope.add_admin_mode = false;
$scope.email_new_admin = "";

 

   $http.get('/api/admin').success(function (data) {
           console.log('/api/admin');
            console.log(data);
             $rootScope.all_admins = data;
           
           });


 $scope.openLightboxModal = function (to_open) {
    $scope.fake_array = [];
    $scope.fake_array.push(to_open);
    Lightbox.openModal( $scope.fake_array, 0);
  };

   $scope.add_admin = function(){
   $scope.add_admin_mode = true;


   }


    

    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }


});

app.controller('manage_indicators_controller', function($scope, $alert,$http,$rootScope,$modal){
  


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
      $scope.please_wait = false;
$scope.nothing_change = true;
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


    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });



$scope.please_wait = false;
$scope.nothing_change = true;
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
            return   angular.isUndefined($scope.year_to_create) || $scope.year_to_create < $scope.max_year_curri_have || $scope.year_to_create == "" ;
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


                  if(!$rootScope.manage_indicators_and_sub_result){
                return true;
            }


                var index;
                $scope.my_num_indicators = [];
                for (index =0;index<  $rootScope.manage_indicators_and_sub_result.length ; index++){
                    if (!$rootScope.manage_indicators_and_sub_result[index].indicator_num || !$rootScope.manage_indicators_and_sub_result[index].indicator_name_e || !$rootScope.manage_indicators_and_sub_result[index].indicator_name_t){
                        return true;
                    }
                }

                for (index =0;index<  $rootScope.manage_indicators_and_sub_result.length ; index++){

                    if($scope.my_num_indicators.indexOf($rootScope.manage_indicators_and_sub_result[index].indicator_num) == -1) {
                        $scope.my_num_indicators.push($rootScope.manage_indicators_and_sub_result[index].indicator_num);
                    }
                    else {

                       return true;

                    }
                }

        

                return false;


      }


      $scope.add_indicator = function(){
        $scope.nothing_change = false;
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
              $scope.nothing_change = false;
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
            $scope.nothing_change = true;
             $rootScope.my_backup_indicators = angular.copy(data);
            $rootScope.manage_indicators_and_sub_result =data;
         });
    }

    $scope.something_change = function(){
        $scope.nothing_change = false;
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
               $scope.close_modal(my_modal);
            $rootScope.my_backup_indicators = angular.copy($rootScope.manage_indicators_and_sub_result);
               $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
                // $rootScope.manage_indicators_and_sub_result[$rootScope.manage_indicators_and_sub_save_indicator.save_index] = angular.copy($rootScope.manage_indicators_indicator_choosen);
                
                 
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
         angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
    }
   $scope.files = [];

       angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
       $scope.my_temp_secret = false;
    
   $scope.$on("fileSelected", function (event, args) {

     
        var extension = args.file.name.split('.');


        $scope.$apply(function () {    
                $scope.files =[];    
            //add the file object to the scope's files collection
            if(args.file.size > 25000000){
                   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                $alert({title:'เกิดข้อผิดพลาด', content:'ไฟล์ที่เลือกมีขนาดมากกว่า 25 MB',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
            else        if(extension[extension.length-1] == 'exe' || extension[extension.length-1] == 'EXE' || extension[extension.length-1] == 'vb' || extension[extension.length-1] == 'VB'
        || extension[extension.length-1] == 'bat' || extension[extension.length-1] == 'BAT'  || extension[extension.length-1] == 'ini' || extension[extension.length-1] == 'INIT' ){
        
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                 
         $alert({title:'เกิดข้อผิดพลาด', content:'ไม่อนุญาติให้อัพโหลดไฟล์นามสกุลดังกล่าว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }
            else{
                     $scope.files.push(args.file);
            }
       
        });
    });


         $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }


    $scope.save_to_server=function(this_modal){
 if($scope.my_temp_secret == false){
            $rootScope.only_object_want_to_change.secret = "0";
        }
        else{
            $rootScope.only_object_want_to_change.secret = "1";
        }


$scope.please_wait = true;

$rootScope.only_object_want_to_change.teacher_id = $rootScope.current_user.user_id;
      var formData = new FormData();
$rootScope.only_object_want_to_change.file_name = $scope.files[0].name;
    
    formData.append("model", angular.toJson($rootScope.only_object_want_to_change));

            formData.append("file" , $scope.files[0]);
        console.log("save to sserver");
console.log($rootScope.only_object_want_to_change);
        $http({
            method: 'PUT',
            url: "/api/evidence/updateevidencefile",

            headers: { 'Content-Type': undefined },


            data:formData,
            transformRequest: angular.indentity 

        }).
        success(function (data, status, headers, config) {
    $rootScope.manage_evidences_still_same();
            $rootScope.manage_evidences_world_evidences = data;
            $scope.close_modal(this_modal);

                $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

                
           
        }).
        error(function (data, status, headers, config) {
                      $scope.please_wait = false;
            $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
    }
});

app.controller('add_new_evidence_controller', function($scope, $alert,$http,$rootScope,request_years_from_curri_choosen_service){
 
    $scope.my_new_evidence = {};
   $scope.my_new_evidence.evidence_real_code = "";
   $scope.my_new_evidence.evidence_name = "";
   $scope.my_new_evidence.secret = false;
   $scope.my_new_evidence_file = [];
 $scope.my_temp_secret_new = false;
 
  
        $scope.please_wait = false;

    $scope.init =function() {
                $scope.please_wait = false;
         $scope.my_temp_secret_new = false;
   $scope.my_new_evidence = {};
   $scope.my_new_evidence.evidence_real_code = "";
   $scope.my_new_evidence.evidence_name = "";
   $scope.my_new_evidence_file = [];
     $scope.my_new_evidence.secret = false;
      $scope.my_new_evidence.evidence_name = "";
      
        var index;
  $rootScope.my_evidence_real_code_we_have_now = [];
             for(index=0;index<$rootScope.manage_evidences_world_evidences.length;index++){
                $rootScope.my_evidence_real_code_we_have_now.push($rootScope.manage_evidences_world_evidences[index].evidence_real_code);
             }

              angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });

}


 $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });
   $scope.$on("fileSelected", function (event, args) {

     
        var extension = args.file.name.split('.');

        $scope.$apply(function () {            
            $scope.my_new_evidence_file = [];
         
                if(args.file.size > 25000000){
                   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                $alert({title:'เกิดข้อผิดพลาด', content:'ไฟล์ที่เลือกมีขนาดมากกว่า 25 MB',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
            else        if(extension[extension.length-1] == 'exe' || extension[extension.length-1] == 'EXE' || extension[extension.length-1] == 'vb' || extension[extension.length-1] == 'VB'
        || extension[extension.length-1] == 'bat' || extension[extension.length-1] == 'BAT'  || extension[extension.length-1] == 'ini' || extension[extension.length-1] == 'INIT' ){
        
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                 
         $alert({title:'เกิดข้อผิดพลาด', content:'ไม่อนุญาติให้อัพโหลดไฟล์นามสกุลดังกล่าว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }
            else{
                   $scope.my_new_evidence_file.push(args.file);
            }

         
      
        });
    });



    $scope.new_evidence_still_not_complete =  function(){
        if (!$scope.my_new_evidence.evidence_real_code || !$scope.my_new_evidence.evidence_name|| $scope.my_new_evidence_file.length ==0){

                return true;
        }
        else{
             if ($scope.my_new_evidence.evidence_real_code <= 0 || $rootScope.my_evidence_real_code_we_have_now.indexOf($scope.my_new_evidence.evidence_real_code) != -1){
                   return true;
                }
            return false;
        }
    }






 $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.save_to_server =function(my_modal){
        $scope.please_wait = true;
        if($scope.my_temp_secret_new  == false){
            $scope.my_new_evidence.secret = "0";
        }
        else{
            $scope.my_new_evidence.secret = "1";
        }



      var formData = new FormData();
$scope.my_new_evidence.file_name = $scope.my_new_evidence_file[0].name;
$scope.my_new_evidence.curri_id =   $rootScope.manage_evidence_curri_id_now;
$scope.my_new_evidence.aca_year = $rootScope.manage_evidence_year_now;
$scope.my_new_evidence.indicator_num = $rootScope.manage_evidence_indicator_num;
$scope.my_new_evidence.teacher_id = $rootScope.current_user.user_id;

    
    formData.append("model", angular.toJson($scope.my_new_evidence));
    formData.append("file" , $scope.my_new_evidence_file[0]);


        $http({
            method: 'PUT',
            url: "/api/evidence/newevidence",

            headers: { 'Content-Type': undefined },


            data:formData,
            transformRequest: angular.indentity 

        }).
        success(function (data, status, headers, config) {
    
                $rootScope.manage_evidences_still_same();
              $rootScope.manage_evidences_world_evidences = data;
              console.log("update manage_evidences_world_evidences");
              console.log( $rootScope.manage_evidences_world_evidences);
               $scope.close_modal(my_modal);
                $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

               
           
        }).
        error(function (data, status, headers, config) {
                      $scope.please_wait = false;
            $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
    }
});


app.controller('add_new_primary_controller', function($scope, $alert,$http,$rootScope,request_years_from_curri_choosen_service){
 
    $scope.my_new_evidence = {};
   $scope.my_new_evidence.evidence_real_code = "";
   $scope.my_new_evidence.evidence_name = "";
   $scope.my_new_evidence.secret = false;
   $scope.my_new_evidence_file = [];
 $scope.my_temp_secret_new = false;


$scope.primary_choosen = {};

        $scope.please_wait = false;
    $scope.init =function() {
                $scope.please_wait = false;
         $scope.my_temp_secret_new = false;
   $scope.my_new_evidence = {};
   $scope.my_new_evidence.evidence_real_code = "";
   $scope.my_new_evidence.evidence_name = "";
   $scope.my_new_evidence_file = [];
     $scope.my_new_evidence.secret = false;
      $scope.my_new_evidence.evidence_name = "";

       $scope.primary_choosen = {};
$scope.my_new_evidence.primary_choosen = {};

  var index;
  $rootScope.my_evidence_real_code_we_have_now = [];
             for(index=0;index<$rootScope.manage_evidences_world_evidences.length;index++){
                $rootScope.my_evidence_real_code_we_have_now.push($rootScope.manage_evidences_world_evidences[index].evidence_real_code);
             }

}
 $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });
   $scope.$on("fileSelected", function (event, args) {

     
        var extension = args.file.name.split('.');

        $scope.$apply(function () {            
            $scope.my_new_evidence_file = [];
            

                if(args.file.size > 25000000){
                   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                $alert({title:'เกิดข้อผิดพลาด', content:'ไฟล์ที่เลือกมีขนาดมากกว่า 25 MB',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
            else        if(extension[extension.length-1] == 'exe' || extension[extension.length-1] == 'EXE' || extension[extension.length-1] == 'vb' || extension[extension.length-1] == 'VB'
        || extension[extension.length-1] == 'bat' || extension[extension.length-1] == 'BAT'  || extension[extension.length-1] == 'ini' || extension[extension.length-1] == 'INIT' ){
        
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                 
         $alert({title:'เกิดข้อผิดพลาด', content:'ไม่อนุญาติให้อัพโหลดไฟล์นามสกุลดังกล่าว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }

            else{
                $scope.my_new_evidence_file.push(args.file);
            }
            
        
        });
    });





    $scope.new_evidence_still_not_complete =  function(){
        if (!$scope.primary_choosen || !$scope.my_new_evidence.evidence_real_code ||  $scope.my_new_evidence_file.length ==0){

                return true;
        }
        else if(!$scope.primary_choosen.evidence_name ){
            return true;
        }
        else{
             if ($scope.my_new_evidence.evidence_real_code <= 0 || $rootScope.my_evidence_real_code_we_have_now.indexOf($scope.my_new_evidence.evidence_real_code) != -1){
                   return true;
                }
            return false;
        }
    }



 $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.save_to_server =function(my_modal){
                $scope.please_wait = true;
        if($scope.my_temp_secret_new  == false){
            $scope.my_new_evidence.secret = "0";
        }
        else{
            $scope.my_new_evidence.secret = "1";
        }



      var formData = new FormData();
// $scope.my_new_evidence.file_name = $scope.my_new_evidence_file[0].name;
// $scope.my_new_evidence.curri_id =   $rootScope.manage_evidence_curri_id_now;
// $scope.my_new_evidence.aca_year = $rootScope.manage_evidence_year_now;

// $scope.my_new_evidence.indicator_num = $rootScope.manage_evidence_indicator_num;
$scope.primary_choosen.file_name = $scope.my_new_evidence_file[0].name;
$scope.primary_choosen.curri_id =   $rootScope.manage_evidence_curri_id_now;
    $scope.primary_choosen.aca_year = $rootScope.manage_evidence_year_now;
    $scope.primary_choosen.indicator_num = $rootScope.manage_evidence_indicator_num;
$scope.primary_choosen.secret = $scope.my_new_evidence.secret;
$scope.primary_choosen.evidence_real_code =   $scope.my_new_evidence.evidence_real_code;
$scope.primary_choosen.teacher_id = $rootScope.current_user.user_id;


    formData.append("model", angular.toJson( $scope.primary_choosen));
    formData.append("file" , $scope.my_new_evidence_file[0]);

        $http({
            method: 'PUT',
            url: "/api/evidence/newprimaryevidence",

            headers: { 'Content-Type': undefined },


            data:formData,
            transformRequest: angular.indentity 

        }).
        success(function (data, status, headers, config) {
    
             $rootScope.manage_evidences_still_same();
              $rootScope.manage_evidences_world_evidences = data;
               $scope.close_modal(my_modal);
                $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

               
           
        }).
        error(function (data, status, headers, config) {
                      $scope.please_wait = false;
            $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
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
$rootScope.my_evidence_real_code_we_have_now =[];

$rootScope.my_all_primary_evidences_responsible = [];
$scope.nothing_change =true;

$rootScope.manage_evidences_still_same = function(){
    $scope.nothing_change =true;
}

$scope.sub_date = function(this_date) {
    var res = this_date.substring(0, 10);
    console.log(res);
    return res;
}

 $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    

   $scope.remove_evidence = function(index_evidence_to_remove) {
    console.log("remove_evidence");
   console.log($rootScope.manage_evidences_world_evidences[index_evidence_to_remove].primary_evidence_num) ;

    if( ! $rootScope.manage_evidences_world_evidences[index_evidence_to_remove].primary_evidence_num || $rootScope.manage_evidences_world_evidences[index_evidence_to_remove].primary_evidence_num <=0){
            $rootScope.manage_evidences_world_evidences.splice(index_evidence_to_remove, 1);
            $scope.nothing_change =false;
    }
else{
   

          $alert({title:'เกิดข้อผิดพลาด', content:'ไม่สามารถลบหลักฐานพื้นฐานได้',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});  
    }
}

    $scope.something_change= function(){
        $scope.nothing_change = false;
    }

$scope.still_not_choose_complete = function(){

     if(!$rootScope.manage_evidences_world_evidences){
        return true;
    }

    // if($rootScope.manage_evidences_world_evidences.length ==0){
    //     return true;
    // }

  var index;
  $scope.all_evidence_real_code = [];
  for(index = 0; index<$rootScope.manage_evidences_world_evidences.length ;index++){
    if(!$rootScope.manage_evidences_world_evidences[index].evidence_real_code ){
        return true;
    }
    else if($rootScope.manage_evidences_world_evidences[index].evidence_real_code <= 0){
        return true;
    }
    else{
        if( $scope.all_evidence_real_code.indexOf($rootScope.manage_evidences_world_evidences[index].evidence_real_code)!=-1){
            return true;
        }
        $scope.all_evidence_real_code.push($rootScope.manage_evidences_world_evidences[index].evidence_real_code);
    }
  }




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

    $scope.send_this = {};
    $scope.send_this.curri_id = $scope.curri_choosen.curri_id;
    $scope.send_this.aca_year = $scope.year_choosen.aca_year;
    $scope.send_this.indicator_num  = $scope.indicator_choosen.indicator_num;
    $scope.send_this.teacher_id = $rootScope.current_user.user_id;

    $http.post(
             '/api/primaryevidence/getOnlyNameAndId',
             JSON.stringify($scope.send_this),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {

                console.log("my_all_primary_evidences_responsible");

             $rootScope.my_all_primary_evidences_responsible = data;
                            console.log(data);
         });

}



$scope.go_to_import = function(){
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
                       $rootScope.my_evidence_real_code_we_have_now =[];
                       $scope.nothing_change =true;
                         $scope.all_curri_that_have_privileges = [];
                                         $scope.corresponding_aca_years = [];
                 $scope.corresponding_indicators = [];
  $scope.$parent.scan_only_privilege_curri('3',$scope.all_curri_that_have_privileges);
}


    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    
       $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      $scope.nothing_change =true;
              request_years_from_curri_choosen_service.async($scope.curri_choosen,3,2).then(function(data) {
        
            $scope.corresponding_aca_years = data;
            $scope.corresponding_indicators = [];
  
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
            $scope.nothing_change =true;
              $rootScope.manage_evidences_world_evidences = data;
              console.log("manage_evidences_world_evidences");
              console.log($rootScope.manage_evidences_world_evidences);
            $scope.choose_not_complete =false;
             var index;
  $rootScope.my_evidence_real_code_we_have_now = [];
             for(index=0;index<$rootScope.manage_evidences_world_evidences.length;index++){
                $rootScope.my_evidence_real_code_we_have_now.push($rootScope.manage_evidences_world_evidences[index].evidence_real_code);
             }
             

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
            $scope.nothing_change =true;
              $scope.corresponding_indicators = data;
            // $scope.get_all_teachers();

         });

    }

    //   $scope.remove_evidence = function(index_evidence_to_remove) { 
    //   $rootScope.manage_evidences_world_evidences.splice(index_evidence_to_remove, 1);     

    // }
    $scope.save_to_server = function(my_modal){
        
        if($rootScope.manage_evidences_world_evidences.length ==0){
            $scope.to_sent = {};
             $scope.to_sent.curri_id = $scope.curri_choosen.curri_id;
             $scope.to_sent.aca_year = $scope.year_choosen.aca_year;
            $scope.to_sent.indicator_num = $scope.indicator_choosen.indicator_num;
             $scope.to_sent.evidence_code = 0;
             $rootScope.manage_evidences_world_evidences.push( $scope.to_sent);
        }
        console.log("save_to_server");
        console.log($rootScope.manage_evidences_world_evidences);


        $http.put(
             '/api/evidence/updateevidence',
             JSON.stringify($rootScope.manage_evidences_world_evidences),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
              $scope.close_modal(my_modal);
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

app.controller('manage_sub_indicators_controller', function($scope, $alert,$http,$rootScope,ngDialog,$modalBox){
$scope.please_wait = false;
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

    $scope.nothing_change = true;

             $scope.still_not_choose_complete_sub =function(){

            if(!$rootScope.manage_indicators_indicator_choosen){
                return true;
            }

            if($rootScope.manage_indicators_indicator_choosen.sub_indicator_list.length ==0){
                return true;
            }

            var index;
            for(index=0;index<$rootScope.manage_indicators_indicator_choosen.sub_indicator_list.length;index++){
                if(!$rootScope.manage_indicators_indicator_choosen.sub_indicator_list[index].sub_indicator_num || !$rootScope.manage_indicators_indicator_choosen.sub_indicator_list[index].sub_indicator_name  ){
                    return true;
                }
            }


            $scope.my_num_indicators = [];
            for (index =0;index<  $rootScope.manage_indicators_indicator_choosen.sub_indicator_list.length ; index++){

                    if($scope.my_num_indicators.indexOf($rootScope.manage_indicators_indicator_choosen.sub_indicator_list[index].sub_indicator_num) == -1) {
                        $scope.my_num_indicators.push($rootScope.manage_indicators_indicator_choosen.sub_indicator_list[index].sub_indicator_num);
                    }
                    else {

                       return true;

                    }
                }

             if(angular.equals($rootScope.manage_indicators_and_sub_save_indicator.save_content,$rootScope.manage_indicators_indicator_choosen)==true){
           
            return true;
             }

             return false;

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
   $scope.nothing_change = true;
    }




    $scope.$on("modal.hide", function (event, args) {

    $scope.start_ka();
      
    });

  $scope.$on("modal.show", function (event, args) {
    $scope.please_wait = false;
                 $scope.start_ka();
    });



     $scope.add_sub_indicator = function(){

         $rootScope.manage_indicators_indicator_choosen.sub_indicator_list.push({
            "aca_year":$rootScope.manage_indicators_and_subs_year_choosen
            ,"indicator_num":$rootScope.manage_indicators_indicator_choosen.indicator_num,
            "sub_indicator_num":"","sub_indicator_name":""});

        $scope.nothing_change = false;
      }


      $scope.remove_sub_indicator = function(index_sub_indicator_to_remove) { 
          $scope.nothing_change = false;
      $rootScope.manage_indicators_indicator_choosen.sub_indicator_list.splice(index_sub_indicator_to_remove, 1);     
      console.log($rootScope.manage_indicators_indicator_choosen);
    }

        $scope.close_modal = function(my_modal){
        $scope.start_ka();
        my_modal.$hide();
    }


$scope.start_ka = function(){
        console.log("close ka");
        $rootScope.manage_indicators_indicator_choosen = angular.copy($rootScope.manage_indicators_and_sub_save_indicator.save_content);
         $rootScope.manage_indicators_and_sub_result[$rootScope.manage_indicators_and_sub_save_indicator.save_index] =  angular.copy($rootScope.manage_indicators_indicator_choosen);
      $scope.nothing_change = true;
}

 $scope.ask_to_save_to_server = function (this_modal) {
                var boxOptions = {
                content: 'หากคุณกดยืนยัน ตัวบ่งชี้หลักจะถูกบันทึกไปตามค่าขณะนี้ด้วย<br>(ซึ่งคุณสามารถแก้ไขภายหลังได้)',
                title:'แจ้งเตือน',
                theme:'danger',
                boxType: 'confirm',
                backdrop:'static',
                confirmText:'ยืนยัน',
                cancelText:'ยกเลิก',
                effect:'bounce-in',
                afterConfirm:function(){ $scope.please_wait = true; $scope.save_to_server(this_modal);},
                }
                $modalBox(boxOptions);
            }

    $scope.save_to_server = function(my_modal){



        // $rootScope.manage_indicators_indicator_choosen.aca_year = $rootScope.manage_indicators_year_to_create;

        // var index;
        // for(index=0;index<$rootScope.manage_indicators_indicator_choosen.sub_indicator_list.length;index++){
        //     $rootScope.manage_indicators_indicator_choosen.sub_indicator_list[index].aca_year = $rootScope.manage_indicators_year_to_create;
        // }
        console.log($rootScope.manage_indicators_and_sub_result);
        $scope.to_sent = angular.copy($rootScope.manage_indicators_and_sub_result);
        $scope.to_sent[$rootScope.manage_indicators_and_sub_save_indicator.save_index] = angular.copy($rootScope.manage_indicators_indicator_choosen);
        // $scope.to_sent.aca_year = $rootScope.manage_indicators_year_to_create;



                var index;
          var sub_index;
 for (index = 0; index < $scope.to_sent.length; index++) {
    $scope.to_sent[index].aca_year = $rootScope.manage_indicators_year_to_create;

    if(angular.isUndefined($scope.to_sent[index].sub_indicator_list)){
        $scope.to_sent[index].sub_indicator_list = [];
    }

    for(sub_index =0; sub_index < $scope.to_sent[index].sub_indicator_list.length ; sub_index++ ){
        $scope.to_sent[index].sub_indicator_list[sub_index].aca_year =$rootScope.manage_indicators_year_to_create; 
    }
 }


        $http.put(
             '/api/indicatorsubIndicator/saveindicator',
             JSON.stringify($scope.to_sent),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {

               $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

                $rootScope.manage_indicators_and_sub_result = angular.copy( $scope.to_sent);
                $rootScope.my_backup_indicators= angular.copy( $scope.to_sent);
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
        $scope.go_request = false;
                 $scope.corresponding_indicators = {};
                 $scope.nothing_change = true;
$scope.init =function() {
     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {};
               $scope.indicator_choosen= {};

       
                 $scope.corresponding_indicators = [];

                 $scope.nothing_change = true;
                   $scope.go_request = false;
}



    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });



      $scope.still_not_choose_complete =function(){


if($scope.choose_not_complete==false){
var index;
for (index =0;index< $scope.result.length ; index++){
     if(!$scope.result[index].evidence_name ){
          return true;

              }

}
          

       }

        return false;
      }




$scope.add_primary_evidence = function(){
      $scope.nothing_change = false;
    console.log($scope.year_choosen.aca_year);

         $scope.result.push({ "evidence_name":"","just_create":true,"curri_id":"0","aca_year":$scope.year_choosen,"indicator_num":$scope.indicator_choosen.indicator_num
});
      }

    $scope.remove_primary_evidence = function(index_primary_evidence_to_remove) { 
              $scope.nothing_change = false;
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
               $scope.nothing_change = true;
                 $scope.go_request = true;

         });

    }

  $scope.find_primary_evidences = function(){

          console.log("find_primary_evidences");

if($scope.go_request == true){
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
            $scope.nothing_change = true;


         });

    }

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
             $scope.close_modal(my_modal);
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



app.controller('manage_primary_evidences_president_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {};
               $scope.indicator_choosen= {};

                $scope.corresponding_aca_years = [];
                 $scope.corresponding_indicators = [];
                 $scope.nothing_change = true;
               $scope.all_curri_that_have_privileges = [];
      $rootScope.curri_that_be_president_in($scope.all_curri_that_have_privileges);
}


    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {};
               $scope.indicator_choosen= {};
 $scope.corresponding_aca_years = [];
       $scope.corresponding_indicators = {};
     $scope.nothing_change = true;

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
    if(!$scope.all_teachers){
        return ;
    }
    for(index=0;index<$scope.all_teachers.length;index++){
        if($scope.all_teachers[index].teacher_id == ask_id){
            return $scope.all_teachers[index].t_name;
        }
    }
}



        $scope.sendCurriAndGetYears = function () {
     


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
                  $scope.choose_not_complete =true;
                $scope.year_choosen = {}
                $scope.indicator_choosen= {};
              
                      request_years_from_curri_choosen_service.async($scope.curri_choosen,999).then(function(data) {
 $scope.corresponding_indicators = [];
                    $scope.corresponding_aca_years = data;
                    $scope.nothing_change = true;

         });

           
          });


    }

     $scope.add_primary_evidence = function(){
         console.log("add");
        // $scope.new_indicator = {"sub_indicator_list":[]
        // ,"aca_year":$scope.year_choosen
        // ,"indicator_num":$rootScope.manage_indicators_and_sub_result.length+1
        // ,"indicator_name_t":"","indicator_name_e":""}
  $scope.nothing_change = false;

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
 $scope.nothing_change = false;
    if(my_obj.status == "2"){
        my_obj.by_pass="1";
    }
}

      $scope.still_not_choose_complete =function(){


        if(!$scope.result){
            return true;
        }
        var index;
        for (index =0;index< $scope.result.length ; index++){

            if($scope.result[index].status != "3" &&$scope.result[index].status != "7" ){

             if(!$scope.result[index].evidence_name || !$scope.result[index].teacher_id){
                  return true;

                      }
            }

        }
                  

        return false;
      }


$scope.send_email = function(primary_obj){

if(angular.isUndefined(primary_obj.teacher_id)){
      $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาเลือกผู้รับผิดชอบหลักฐานก่อนส่ง',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});

}else{
    $scope.to_sent = {};
    $scope.to_sent.curri_id = primary_obj.curri_id;
    $scope.to_sent.primary_evidence_num = primary_obj.primary_evidence_num;
           $http.post(
             '/api/primaryevidence/sendmail',
             JSON.stringify($scope.to_sent),
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
             
     $alert({title:'เกิดข้อผิดพลาด', content:'ส่ง Email แจ้งเตือนไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     

  }); 
}



}
      $scope.remove_primary_evidence = function(index_primary_evidence_to_remove) { 
        console.log("remove");
            $scope.nothing_change = false;
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
         
            $scope.nothing_change = true;

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
             $scope.nothing_change = true;
           
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

               $scope.close_modal(my_modal);
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


app.controller('result_survey_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                    $scope.suggestion = "";
}
  
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
 $scope.suggestion = "";


});


app.controller('answer_survey_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                    $scope.suggestion = "";
}
  
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
 $scope.suggestion = "";



 $scope.still_not_complete=function(){
    if(! $scope.suggestion){
        return true;
    }

    var index;
    for(index =0 ;index<$rootScope.manage_survey_questionare_set.length ;index++){
        if (!$rootScope.manage_survey_questionare_set[index].answer){
            return true;
        }
    }
 }
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
  $scope.save_to_server = function(my_modal){

        $rootScope.manage_survey_questionare_set.push({"suggestion":$scope.suggestion}) ;
        $http.put(
             '/api/questionareanswer',
             JSON.stringify( $rootScope.manage_survey_questionare_set),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.close_modal(my_modal);
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


app.controller('manage_lab_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                          $rootScope.manage_lab_my_world_wide_labs = [];
                    $scope.nothing_change = true;
                
                       $scope.all_curri_that_have_privileges = [];
                             $scope.corresponding_aca_years = [];   
  $scope.$parent.scan_only_privilege_curri('10',$scope.all_curri_that_have_privileges);
}

$rootScope.manage_lab_still_same = function(){
     $scope.nothing_change = true;
}
    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

  
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
  $scope.nothing_change = true;

 $rootScope.manage_lab_my_world_wide_labs = [];
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
         $scope.nothing_change = true;
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen,10,2).then(function(data) {
$scope.corresponding_indicators = [];
            $scope.corresponding_aca_years = data;


          });


    }

    $scope.go_to_create_lab =function(){
           $http.post(
             '/api/personnel/gettnameandid',
             JSON.stringify($scope.curri_choosen.curri_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.manage_lab_all_personnels_in_curri = data;
              
        $rootScope.manage_lab_curri_id = $scope.curri_choosen.curri_id;
        $rootScope.manage_lab_aca_year = $scope.year_choosen.aca_year;

});

    }

    $scope.initial_my_selected = function(){
        console.log("initial_my_selected");
        $scope.my_manage_lab = $rootScope.manage_lab_fix_this_lab.officer;
    }
      $scope.go_to_fix_lab = function(lab_to_fix){
     
       
      
              $http.post(
             '/api/personnel/gettnameandid',
             JSON.stringify($scope.curri_choosen.curri_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            console.log(data);
       
            $rootScope.manage_lab_all_personnels_in_curri = data;
 
             $rootScope.manage_lab_fix_this_lab = angular.copy(lab_to_fix);
             $rootScope.manage_lab_fix_this_lab_init = [];
             // var index;
             //      console.log($rootScope.manage_lab_fix_this_lab);
             // for(index = 0; index<$rootScope.manage_lab_fix_this_lab.officer.length;index++ ){
             //    $rootScope.manage_lab_fix_this_lab_init.push($rootScope.manage_lab_fix_this_lab.officer[index].personnel_id);
             // }
             //            console.log($rootScope.manage_lab_fix_this_lab_init);
    


             var index;
             var inside_index;

        for(index =0;index<$rootScope.manage_lab_fix_this_lab.officer.length;index++){
            for(inside_index=0;inside_index<$rootScope.manage_lab_all_personnels_in_curri.length;inside_index++){
                if($rootScope.manage_lab_all_personnels_in_curri[inside_index].user_id  == $rootScope.manage_lab_fix_this_lab.officer[index].user_id){
                          $rootScope.manage_lab_fix_this_lab_init.push($rootScope.manage_lab_all_personnels_in_curri[inside_index]);
                }
          
            }
        }

         });
console.log('manage_lab_fix_this_lab_init')
        console.log($rootScope.manage_lab_fix_this_lab_init);
          console.log('$rootScope.manage_lab_all_personnels_in_curri');
        console.log($rootScope.manage_lab_all_personnels_in_curri);
    }







    $scope.remove_lab = function(index_to_remove){
        $scope.nothing_change = false;
        $rootScope.manage_lab_my_world_wide_labs.splice(index_to_remove, 1);   
    }



    $scope.find_information = function(){

          console.log("find_information");
        console.log($scope.year_choosen);

        $http.post(
             '/api/lablist/getlablist',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.nothing_change = true;
               $rootScope.manage_lab_my_world_wide_labs = data;
         
             $scope.choose_not_complete = false;
             
            
    
         });

    }
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){


        if($rootScope.manage_lab_my_world_wide_labs.length == 0){
            console.log("legnth = 0")
            $scope.to_sent  = {};
            $scope.to_sent.curri_id  = $scope.curri_choosen.curri_id;
            $scope.to_sent.aca_year = $scope.year_choosen.aca_year;
   
            $rootScope.manage_lab_my_world_wide_labs.push($scope.to_sent);

        }

        console.log("save_to_server");
        console.log($rootScope.manage_lab_my_world_wide_labs);
        $http.put(
             '/api/lablist/delete',
             JSON.stringify($rootScope.manage_lab_my_world_wide_labs),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.close_modal(my_modal);
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

app.controller('manage_survey_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                    $scope.nothing_change = true;
                      $rootScope.manage_survey_my_world_wide_surveys = [];
                       $scope.all_curri_that_have_privileges = [];
                       $scope.corresponding_aca_years = [];
  $scope.$parent.scan_only_privilege_curri('5',$scope.all_curri_that_have_privileges);
}

$rootScope.manage_survey_still_same = function(){
    $scope.nothing_change =true;
}


   $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
  $scope.nothing_change = true;

  $rootScope.manage_survey_my_world_wide_surveys = [];
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
         $scope.nothing_change = true;
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen,5,2).then(function(data) {
$scope.corresponding_indicators = [];
            $scope.corresponding_aca_years = data;


          });


    }



$scope.right_target = function(targets){
    var i;


    for(i= 0 ;i<targets.length;i++ ){
        if( $rootScope.current_user.user_type == targets[i]){
            return true;
        }
       
    }
    return false;
}

    $scope.go_to_answer = function(this_survey){

           $http.post(
             '/api/questionareanswer',
             JSON.stringify(this_survey.questionare_set_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $rootScope.manage_survey_questionare_set = data;
         });



        $rootScope.manage_survey_questionare_to_answer = this_survey;
//         $rootScope.manage_survey_questionare_set = [{"answer":"","aquestionare_set_id":1,"questionare_question_id":1,"detail":"ความน่าสนใจของเนื้อหารายวิชา"},{"answer":"","questionare_set_id":1,"questionare_question_id":2,"detail":"อาจารย์ผู้สอน สอนสบายๆ"},{"answer":"","questionare_set_id":1,"questionare_question_id":3,"detail":"งานที่มอบหมาย มีความเหมาะสมต่อภาระของนักศึกษา"},{"answer":"","questionare_set_id":1,"questionare_question_id":4,"detail":"ระยะเวลาที่สอนต่อครั้งมีความเหมาสม"},{"answer":"","questionare_set_id":1,"questionare_question_id":5,"detail":"ความยากของข้อสอบ Final"}]

// ;



    }

       $scope.go_to_result = function(this_survey){

          $http.post(
             '/api/questionareresult',
             JSON.stringify(this_survey.questionare_set_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.manage_survey_result = data;
         });


        $rootScope.manage_survey_questionare_of_result = this_survey;
//         $rootScope.manage_survey_result= {"suggestion":["อยากให้ลดปริมาณงานน้อยๆ ลง","เพิ่มคะแนนเก็บเยอะๆค่ะ","อยากให้อาจารย์สอนแล้วเลิกเร็วแบบนี้ไปตลอด","คิดไม่ออก","ไม่มีความคิดเห็น"],
// "main_result_list":[{"name":"ความน่าสนใจของเนื้อหารายวิชา","answer":[11,0,0,0,2,0,3]},
// {"name":"อาจารย์ผู้สอน สอนสบายๆ","answer":[10,2,0,0,0,1,3]},
// {"name":"งานที่มอบหมาย มีความเหมาะสมต่อภาระของนักศึกษา","answer":[10,0,3,0,0,0,3]},
// {"name":"ระยะเวลาที่สอนต่อครั้งมีความเหมาสม","answer":[11,0,0,1,0,1,3]},
// {"name":"ความยากของข้อสอบ Final","answer":[10,2,0,0,0,1,3]}]};






    }

    $scope.remove_qestionare = function(index_to_remove){
        $scope.nothing_change = false;
        $rootScope.manage_survey_my_world_wide_surveys.splice(index_to_remove, 1);   
    }
    $scope.go_to_create_survey = function(){
        console.log("corresponding_aca_years");
          console.log( $scope.corresponding_aca_years);
        console.log( $scope.corresponding_aca_years.aca_year);
        $rootScope.manage_survey_curri_id_now = $scope.curri_choosen.curri_id;
        $rootScope.manage_survey_aca_year_now = $scope.year_choosen.aca_year;

    }

    $scope.find_information = function(){

          console.log("find_information");
        console.log($scope.year_choosen);

        $http.post(
             '/api/questionare/getquestionareset',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {

             $scope.nothing_change = true;
               $rootScope.manage_survey_my_world_wide_surveys = data;
                   console.log('$rootScope.manage_survey_my_world_wide_surveys')
            console.log($rootScope.manage_survey_my_world_wide_surveys)
             $scope.choose_not_complete = false;
             
            
    
         });
// $scope.result=[{"target":["นักศึกษา"],"t_name":"อาจารย์บัณฑิต พัสยา","questionare_set_id":1,"name":"ความคิดเห็นต่อเนื้อหารายวิชา 01076573 Information Storage and Retrieval","curri_id":"21","aca_year":2558,"date":"20/11/2558","personnel_id":"00007"},{"target":["นักศึกษา"],"t_name":"อาจารย์วิบูลย์ พร้อมพานิชย์","questionare_set_id":3,"name":"ความคิดเห็นต่อการเรียนการสอนวิชา 01076234 Computer programming 1","curri_id":"21","aca_year":2558,"date":"30/11/2558","personnel_id":"00001"},{"target":["นักศึกษา","บริษัท","ศิษย์เก่า","อาจารย์"],"t_name":"รศ.ดร.ศุภมิตร จิตตะยโศธร","questionare_set_id":4,"name":"แบบสำรวจความต้องการในการจัดสัมมนาเกี่ยวกับเรื่องฐานข้อมูลเชิงเวลาแบบไม่แน่นอน","curri_id":"21","aca_year":2558,"date":"01/01/2559","personnel_id":"00009"},{"target":["เจ้าหน้าที่","นักศึกษา","บริษัท","ศิษย์เก่า","อาจารย์"],"t_name":"รศ.ดร.เกียรติกูล เจียรนัยธนะกิจ","questionare_set_id":5,"name":"แบบสำรวจสภาพการใช้งานของสุขภัณฑ์ในภาควิชา","curri_id":"21","aca_year":2558,"date":"02/12/2558","personnel_id":"00002"},{"target":["นักศึกษา","อาจารย์"],"t_name":"ผศ.ดร.ชุติเมษฏ์ ศรีนิลทา","questionare_set_id":6,"name":"แบบสำรวจความต้องการในการจัดให้มีกิจกรรมปัจฉิมนิเทศของภาค","curri_id":"21","aca_year":2558,"date":"22/12/2558","personnel_id":"00004"}];
//  $scope.choose_not_complete = false;

    }
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){


        if($rootScope.manage_survey_my_world_wide_surveys.length == 0){
            console.log("legnth = 0")
            $scope.to_sent  = {};
            $scope.to_sent.curri_id  = $scope.curri_choosen.curri_id;
            $scope.to_sent.aca_year = $scope.year_choosen.aca_year;
            $scope.to_sent.questionare_set_id = 0;
            $rootScope.manage_survey_my_world_wide_surveys.push($scope.to_sent);

        }

        console.log("save_to_server");
        console.log($rootScope.manage_survey_my_world_wide_surveys);
        $http.put(
             '/api/questionare/delete',
             JSON.stringify($rootScope.manage_survey_my_world_wide_surveys),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.close_modal(my_modal);
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

app.controller('show_edit_album_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,Lightbox) {
  $scope.openLightboxModal = function (index) {

    Lightbox.openModal($rootScope.manage_album_show_this_album.pictures, index);
  }

  $scope.my_pictures = {};
        $scope.please_wait = false;
  $scope.close_modal = function(my_modal){
            $scope.please_wait = false;
        $scope.my_pictures.flow.files = [];
        my_modal.$hide();
    }


  $scope.remove_pic = function(index_to_remove){
        $rootScope.manage_album_show_this_album.pictures.splice(index_to_remove,1);
  }

  $scope.show_my_pictures=function(){


       $scope.to_del = [];
var index;
     for(index=0;index<$scope.my_pictures.flow.files.length;index++){
        console.log("loop");
        console.log($scope.my_pictures.flow.files[index].name);
        console.log($scope.my_pictures.flow.files[index].size);
        if ($scope.my_pictures.flow.files[index].size > 2000000){
            console.log("remove file");
            console.log($scope.my_pictures.flow.files[index].name);
             $scope.to_del.push($scope.my_pictures.flow.files[index]);

        }
    
     }
        
     console.log(" $scope.to_del");
     console.log( $scope.to_del);

    for(index=0;index<$scope.to_del.length;index++){
       $scope.my_pictures.flow.files.splice( $scope.my_pictures.flow.files.indexOf($scope.to_del[index]),1);
    }


}
    $scope.still_not_choose_complete = function(){
               if(!$scope.my_pictures.flow){
            return true;
        }

        if(!$rootScope.manage_album_show_this_album){
            return true;
        }
        if(!$rootScope.manage_album_show_this_album.name ){
            return true;
        }
        else{
                if($scope.my_pictures.flow.files.length ==0 && $rootScope.manage_album_show_this_album.pictures.length ==0){
                    return true;
                }

          var index;
          for(index = 0 ;index<$rootScope.manage_album_show_this_album.pictures.length ;index++){
            if(!$rootScope.manage_album_show_this_album.pictures[index].caption){
                return true;
            }
            
          }

            for(index = 0 ;index<$scope.my_pictures.flow.files.length ;index++){
            if(!$scope.my_pictures.flow.files[index].caption){
                return true;
            }
            
          }
            return false;
        }


return false; 
    }
   $scope.save_to_server = function(my_modal) {
   $scope.please_wait = true;
      var formData = new FormData();


    console.log("this is all");
    console.log($scope.my_pictures.flow.files);
        var index = 0;
        for (index = 0 ;index< $scope.my_pictures.flow.files.length;index++){
            $scope.my_obj = {};
            $scope.my_obj.gallery_id = 0;
            $scope.my_obj.file_name = $scope.my_pictures.flow.files[index].file.name;
            $scope.my_obj.caption = $scope.my_pictures.flow.files[index].caption;
              $rootScope.manage_album_show_this_album.pictures.push($scope.my_obj);

            formData.append("picture"+(index+1), $scope.my_pictures.flow.files[index].file );
        }
        formData.append("model", angular.toJson($rootScope.manage_album_show_this_album));
        console.log("save")
        console.log($rootScope.manage_album_show_this_album);
   // if($scope.disabled_search == true){
   //    $rootScope.manage_minutes_fix_this_minute.file_name = $scope.my_file[0].name;
   //       formData.append("file", $scope.my_file[0] );
   //  }
     

        $http({
            method: 'PUT',
            url: "/api/gallery/edit",

            headers: { 'Content-Type': undefined },


            data:formData,
            transformRequest: angular.indentity 

        }).
        success(function (data, status, headers, config) {
        $rootScope.manage_album_still_same();
                $rootScope.manage_album_my_world_wide_album =data;
                $scope.close_modal(my_modal);
                $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

              
           
        }).
        error(function (data, status, headers, config) {
            $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
    }

});



app.controller('show_album_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,Lightbox) {
  $scope.openLightboxModal = function (index) {

    Lightbox.openModal($rootScope.manage_album_show_this_album.pictures, index);
  }

 




});

app.controller('manage_album_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,Lightbox) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                    $scope.nothing_change = true;
                      $rootScope.manage_survey_my_world_wide_surveys = [];
                          $scope.all_curri_that_have_privileges = [];
                          $scope.corresponding_aca_years = [];
  $scope.$parent.scan_only_privilege_curri('6',$scope.all_curri_that_have_privileges);
}

$rootScope.manage_album_still_same = function(){
   $scope.nothing_change = true;
}

 $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });
  
   $scope.openLightboxModal = function (index,this_album) {

    Lightbox.openModal(this_album, index);
  }

  $scope.remove_album = function(index_to_del){
    $scope.nothing_change= false;
    $rootScope.manage_album_my_world_wide_album.splice(index_to_del,1)
  }


     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
  $scope.nothing_change = true;


        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
         $scope.nothing_change = true;
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen,6,2).then(function(data) {
$scope.corresponding_indicators = [];
            $scope.corresponding_aca_years = data;


          });


    }

    $scope.go_to_show_album = function(this_album){


        $rootScope.manage_album_curri_id = $scope.curri_choosen.curri_id;
        $rootScope.manage_album_aca_year = $scope.year_choosen.aca_year;
        $rootScope.manage_album_show_this_album = angular.copy(this_album);



    }

  $scope.go_to_create_album = function(){
 $rootScope.manage_album_curri_id = $scope.curri_choosen.curri_id;
        $rootScope.manage_album_aca_year = $scope.year_choosen.aca_year;
    

  }

    $scope.find_information = function(){

          console.log("find_information");
        console.log($scope.year_choosen);

        $http.post(
             '/api/gallery/getgallery',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.nothing_change = true;
               $rootScope.manage_album_my_world_wide_album = data;
         
             $scope.choose_not_complete = false;
             
            
    
         });
// $scope.result=[{"target":["นักศึกษา"],"t_name":"อาจารย์บัณฑิต พัสยา","questionare_set_id":1,"name":"ความคิดเห็นต่อเนื้อหารายวิชา 01076573 Information Storage and Retrieval","curri_id":"21","aca_year":2558,"date":"20/11/2558","personnel_id":"00007"},{"target":["นักศึกษา"],"t_name":"อาจารย์วิบูลย์ พร้อมพานิชย์","questionare_set_id":3,"name":"ความคิดเห็นต่อการเรียนการสอนวิชา 01076234 Computer programming 1","curri_id":"21","aca_year":2558,"date":"30/11/2558","personnel_id":"00001"},{"target":["นักศึกษา","บริษัท","ศิษย์เก่า","อาจารย์"],"t_name":"รศ.ดร.ศุภมิตร จิตตะยโศธร","questionare_set_id":4,"name":"แบบสำรวจความต้องการในการจัดสัมมนาเกี่ยวกับเรื่องฐานข้อมูลเชิงเวลาแบบไม่แน่นอน","curri_id":"21","aca_year":2558,"date":"01/01/2559","personnel_id":"00009"},{"target":["เจ้าหน้าที่","นักศึกษา","บริษัท","ศิษย์เก่า","อาจารย์"],"t_name":"รศ.ดร.เกียรติกูล เจียรนัยธนะกิจ","questionare_set_id":5,"name":"แบบสำรวจสภาพการใช้งานของสุขภัณฑ์ในภาควิชา","curri_id":"21","aca_year":2558,"date":"02/12/2558","personnel_id":"00002"},{"target":["นักศึกษา","อาจารย์"],"t_name":"ผศ.ดร.ชุติเมษฏ์ ศรีนิลทา","questionare_set_id":6,"name":"แบบสำรวจความต้องการในการจัดให้มีกิจกรรมปัจฉิมนิเทศของภาค","curri_id":"21","aca_year":2558,"date":"22/12/2558","personnel_id":"00004"}];
//  $scope.choose_not_complete = false;

    }
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){


        if($rootScope.manage_album_my_world_wide_album.length == 0){
            console.log("legnth = 0")
            $scope.to_sent  = {};
            $scope.to_sent.curri_id  = $scope.curri_choosen.curri_id;
            $scope.to_sent.aca_year = $scope.year_choosen.aca_year;
      
            $rootScope.manage_album_my_world_wide_album.push($scope.to_sent);

        }

        console.log("save_to_server");
        console.log($rootScope.manage_album_my_world_wide_album);
        $http.put(
             '/api/gallery/delete',
             JSON.stringify($rootScope.manage_album_my_world_wide_album),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.close_modal(my_modal);
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



app.controller('import_evidence_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {

   $scope.watch_file = function(path) { 
        window.open(path, '_blank', "width=800, left=230,top=0,height=700");  
    }


 


       

$scope.still_not_write_code = function() {
  
    if(!$scope.evidence_we_want || !$scope.code_we_want ){
         
         return true;
    }
    else if(!$scope.evidence_we_want.evidence_name){
        return true;
    }
    else{
      
       if( $rootScope.my_evidence_real_code_we_have_now.indexOf($scope.code_we_want) != -1){
           
            return true;
        }
         
        return false;
    }
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
              $scope.curri_choosen = {};
                $scope.evidence_we_want = "";
                    $scope.result = {};
$scope.code_we_want = "";
$scope.evidence_we_want = {};

     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {};
                $scope.evidence_we_want = "";
                    $scope.result = {};
                    $scope.code_we_want = "";
$scope.corresponding_aca_years = [];
$scope.all_evidences =[];
 $scope.all_curri_that_have_privileges = [];
  $scope.$parent.scan_only_privilege_curri('3',$scope.all_curri_that_have_privileges);

 var index;
  $rootScope.my_evidence_real_code_we_have_now = [];
             for(index=0;index<$rootScope.manage_evidences_world_evidences.length;index++){
                $rootScope.my_evidence_real_code_we_have_now.push($rootScope.manage_evidences_world_evidences[index].evidence_real_code);
             }
                }
  
   $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });
  
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
     
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen,3,2).then(function(data) {


if($rootScope.manage_evidence_curri_id_now == $scope.curri_choosen.curri_id){
    var i;
    for(i=0;i<data.length;i++){
        if(data[i].aca_year == $rootScope.manage_evidence_year_now ){
            data.splice(i,1);
            break;
        }
    }
}


$scope.all_evidences =[];
            $scope.corresponding_aca_years = data;
   $scope.evidence_we_want = {};
          });


    }

    $scope.find_all_evidences_in_curri_and_year = function(){

          console.log("find_information");
        console.log($scope.year_choosen);

        $http.post(
             '/api/evidence/getbycurriculumacademic',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            console.log("find_all_evidences_in_curri_and_year");
                console.log(data);

              $scope.all_evidences = data;
             $scope.choose_not_complete = false;
       
   
         });

    }
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){

   
$scope.evidence_we_want.curri_id =   $rootScope.manage_evidence_curri_id_now;
$scope.evidence_we_want.aca_year = $rootScope.manage_evidence_year_now;

$scope.evidence_we_want.indicator_num = $rootScope.manage_evidence_indicator_num;
$scope.evidence_we_want.evidence_real_code = $scope.code_we_want;
$scope.evidence_we_want.teacher_id = $rootScope.current_user.user_id;

        
        $http.put(
             '/api/evidence/newevidencefromothers',
             JSON.stringify($scope.evidence_we_want),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.manage_evidences_still_same();
             $rootScope.manage_evidences_world_evidences = data;
              $scope.close_modal(my_modal);
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





app.controller('create_user_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
   $scope.curri_choosen = [];
   $scope.my_type = '';
      $scope.files = [];
$scope.choose_not_complete = true;
        $scope.please_wait = false;
    $http.get('/api/usertype').success(function (data) {
           
                $scope.all_usertype = data;


              });
angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    

   $scope.init =function() {

   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
       $scope.files = [];
   $scope.curri_choosen = [];
      $scope.my_type = '';
               $scope.please_wait = false;
$scope.choose_not_complete = true;
$scope.choose_type = false;
if($rootScope.current_user.user_type != 'ผู้ดูแลระบบ'){

  $scope.all_curri_that_have_privileges = [];
  $scope.$parent.scan_only_privilege_curri('1',$scope.all_curri_that_have_privileges);
}
else{
    $scope.all_curri_that_have_privileges = $rootScope.all_curriculums;
}


          $http.get('/api/usertype').success(function (data) {
              
                $scope.all_usertype = data;


              });

   }

   $scope.still_not_complete = function(){
 
        if($scope.files.length == 0 ){


            return true;
        }
        if(!$scope.my_type){
                    return true;
        }

        return false;
   }
  $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }


$scope.save_to_server = function(my_modal) {
          $scope.please_wait = true;
      var formData = new FormData();
$scope.my_new_user = {};
$scope.my_new_user.curri = $scope.curri_choosen;
$scope.my_new_user.type = $scope.my_type;

    formData.append("model", angular.toJson($scope.my_new_user));

      
        
            formData.append("file" , $scope.files[0]);
     

        $http({
            method: 'POST',
            url: "/api/users/createnewusers",

            headers: { 'Content-Type': undefined },


            data:formData,
            transformRequest: angular.indentity 

        }).
        success(function (data, status, headers, config) {
            if(!data){
            
                  my_modal.$hide();
$scope.init ();
                 $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลสำเร็จ',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
            }
            else{
               
                $rootScope.manage_user_email_duplicate  = data;
                        $scope.init ();
                  my_modal.$hide();
              $alert({title:'ดำเนินการสำเร็จบางส่วน', template:'/alert/mycustomtemplate.html',alertType:'warning',duration:10000,
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
                

        }).
        error(function (data, status, headers, config) {
              $scope.please_wait = false;
               $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
}

   $scope.$on("fileSelected", function (event, args) {

     
        var extension = args.file.name.split('.');

        $scope.$apply(function () {            
            $scope.files = [];
            //add the file object to the scope's files collection

                if(args.file.size > 25000000){
                   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                $alert({title:'เกิดข้อผิดพลาด', content:'ไฟล์ที่เลือกมีขนาดมากกว่า 25 MB',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
            else        if(extension[extension.length-1] == 'exe' || extension[extension.length-1] == 'EXE' || extension[extension.length-1] == 'vb' || extension[extension.length-1] == 'VB'
        || extension[extension.length-1] == 'bat' || extension[extension.length-1] == 'BAT'  || extension[extension.length-1] == 'ini' || extension[extension.length-1] == 'INIT' ){
        
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                 
         $alert({title:'เกิดข้อผิดพลาด', content:'ไม่อนุญาติให้อัพโหลดไฟล์นามสกุลดังกล่าว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }

            else{
                 $scope.files.push(args.file);
            }
           
        });
    });
});

app.controller('create_survey_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                    $scope.my_target = [];
                    $scope.questions =[];
                    $scope.my_survey_name ="";

                $http.get('/api/usertype').success(function (data) {
                console.log("/api/usertype");
                console.log(data);
                $scope.all_usertype = data;


              });
            
}
  
      $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    
    $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                    $scope.my_target = [];
                    $scope.questions =[];
                    $scope.my_survey_name ="";

                $http.get('/api/usertype').success(function (data) {
                     console.log("/api/usertype");
                console.log(data);
                $scope.all_usertype = data;
              });


        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      $scope.my_survey_name ="";
              request_years_from_curri_choosen_service.async($scope.curri_choosen).then(function(data) {
$scope.corresponding_indicators = [];
            $scope.corresponding_aca_years = data;

          });


    }

    $scope.still_not_complete = function(){
                if($scope.questions.length == 0) {
                    return true;
                }

                if(!$scope.my_survey_name){
                    return true;
                }

               if($scope.my_target.length == 0){
                    return true;
                }
                
                var index;
                for(index = 0;index < $scope.questions.length ; index++){
                    if(!$scope.questions[index].detail ){
                        return true;
                    }
                }

                return false;

}
   $scope.add_question = function(){
      
       $scope.questions.push({detail:""});
     }

     $scope.remove_question = function(question_index){
       
       $scope.questions.splice(question_index, 1);   
     }


  
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.save_to_server = function(my_modal){
        $scope.my_new_survey = {};
        $scope.my_new_survey.curri_id =  $rootScope.manage_survey_curri_id_now;
        $scope.my_new_survey.aca_year = $rootScope.manage_survey_aca_year_now;
        $scope.my_new_survey.personnel_id = $rootScope.current_user.user_id;
        $scope.my_new_survey.my_target  = $scope.my_target;
        $scope.my_new_survey.my_questions  =$scope.questions;
        $scope.my_new_survey.name  = $scope.my_survey_name;


        console.log("save_to_server");
        console.log($scope.my_new_survey);
        $http.put(
             '/api/questionare/add',
             JSON.stringify($scope.my_new_survey),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.manage_survey_still_same();
            $rootScope.manage_survey_my_world_wide_surveys =data;
              $scope.close_modal(my_modal);
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


app.controller('create_research_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
       $scope.please_wait = false;
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                   $scope.new_research = {};
  $scope.new_research.name = "";
  $scope.new_research.researcher =[];
  $scope.new_research.year_publish = "";
  $scope.new_research.file = "";

   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });

}

    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    

  $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
  $scope.new_research = {};
  $scope.new_research.name = "";
  $scope.new_research.researcher =[];
  $scope.new_research.year_publish = "";
  $scope.new_research.file = "";

   $scope.please_wait = false;
    $scope.$on("fileSelected", function (event, args) {

     
        var extension = args.file.name.split('.');

        $scope.$apply(function () {            
            $scope.new_research.file = [];
            

                if(args.file.size > 25000000){
                   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                $alert({title:'เกิดข้อผิดพลาด', content:'ไฟล์ที่เลือกมีขนาดมากกว่า 25 MB',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
            else        if(extension[extension.length-1] == 'exe' || extension[extension.length-1] == 'EXE' || extension[extension.length-1] == 'vb' || extension[extension.length-1] == 'VB'
        || extension[extension.length-1] == 'bat' || extension[extension.length-1] == 'BAT'  || extension[extension.length-1] == 'ini' || extension[extension.length-1] == 'INIT' ){
        
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                 
         $alert({title:'เกิดข้อผิดพลาด', content:'ไม่อนุญาติให้อัพโหลดไฟล์นามสกุลดังกล่าว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }
            else{
                 $scope.new_research.file.push(args.file);
            }
           
        });
    });


    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

     $scope.still_not_complete = function(){

        if(! $scope.new_research.name || $scope.new_research.researcher.length ==0 || !$scope.new_research.year_publish || !$scope.new_research.file ){
            return true;
        }
        else{
            if(angular.isNumber($scope.new_research.year_publish)==false){
                return true;


            }

             if($scope.new_research.year_publish <= 0){
                    return true;
                }
             return false;
        }


       
     }
    
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.save_to_server = function(my_modal) {
           $scope.please_wait = true;
 $scope.new_research.curri_id = $rootScope.manage_reseach_my_curri_id_now;
 $scope.new_research.file_name = $scope.new_research.file.name;

 console.log("save_to_server");
 console.log($scope.new_research);
      var formData = new FormData();

    formData.append("model", angular.toJson( $scope.new_research));

     
        
            formData.append("file", $scope.new_research.file[0] );
    

        $http({
            method: 'POST',
            url: "/api/research/newresearch",

            headers: { 'Content-Type': undefined },


            data:formData,
            transformRequest: angular.indentity 

        }).
        success(function (data, status, headers, config) {
    $rootScope.manage_research_still_same();
                $rootScope.manage_research_my_research_now =data;
                $scope.close_modal(my_modal);
                $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

              
           
        }).
        error(function (data, status, headers, config) {
                      $scope.please_wait = false;
            $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
    }


});

app.controller('fix_research_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                               $scope.new_file = [];
                               $scope.disabled_search = false;
  $scope.please_wait = false;

}
  $scope.please_wait = false;
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                     $scope.new_file = [];
                        $scope.disabled_search = false;

    $scope.still_not_complete = function(){

        if(!$rootScope.manage_research_fix_this_research){
            return true;
        }
        if(!$rootScope.manage_research_fix_this_research.name || !$rootScope.manage_research_fix_this_research.year_publish ){
            return true;
        }

        if (angular.isNumber($rootScope.manage_research_fix_this_research.year_publish) == false){
            return true;
        }

        if($rootScope.manage_research_fix_this_research.year_publish <=0){
            return true;
        }

        if($rootScope.manage_research_fix_this_research.researcher.length == 0){
            return true;
        }

        if($scope.disabled_search==true ){
            if($scope.new_file.length == 0){
                return true;
            }
            return false;
        }
        else{
            return false;
        }
    }

    $scope.watch_file = function(research_path){
        if( $scope.disabled_search == false){
            window.open(research_path, '_blank', "width=800, left=230,top=0,height=700"); 
        }
         
    }
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.set_disabled_search = function(){
        console.log("disabled_search");
        console.log($scope.disabled_search);
        $scope.disabled_search = true;
    }
  $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });


  $scope.save_to_server = function(my_modal){
       $scope.please_wait = true;
        console.log("save_to_server");
           
    
          $rootScope.manage_research_fix_this_research.researcher = [];
        var index;
        // for(index =0;index<$rootScope.manage_research_all_teachers_in_curri.length;index++){
        //     if($scope.manage_lab_research_this_research_init.indexOf($rootScope.manage_research_all_teachers_in_curri[index].teacher_id) != -1){
        //         $rootScope.manage_research_fix_this_research.researcher.push($rootScope.manage_research_all_teachers_in_curri[index]);
        //     }
        // }

$rootScope.manage_research_fix_this_research.researcher = $rootScope.manage_lab_research_this_research_init;

          var formData = new FormData();

        formData.append("model", angular.toJson($rootScope.manage_research_fix_this_research));


            if($scope.disabled_search == true){
                 $rootScope.manage_research_fix_this_research.file_name = $scope.new_file[0].name;
        formData.append("file" , $scope.new_file[0]);
 }
            $http({
                method: 'PUT',
                url: "/api/research/edit",

                headers: { 'Content-Type': undefined },


                data:formData,
                transformRequest: angular.indentity 

            }).
            success(function (data, status, headers, config) {
                $rootScope.manage_research_still_same();
                     $rootScope.manage_research_my_research_now = data;
                     $scope.close_modal(my_modal);
                

                    $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                             placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

                   
            }).
            error(function (data, status, headers, config) {
                $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                             placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
            });
       

            
     
    }

        $scope.$on("fileSelected", function (event, args) {

     
        var extension = args.file.name.split('.');

        $scope.$apply(function () {            
             $scope.new_file = [];
            //add the file object to the scope's files collection

             if(args.file.size > 25000000){
                   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                $alert({title:'เกิดข้อผิดพลาด', content:'ไฟล์ที่เลือกมีขนาดมากกว่า 25 MB',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
            else        if(extension[extension.length-1] == 'exe' || extension[extension.length-1] == 'EXE' || extension[extension.length-1] == 'vb' || extension[extension.length-1] == 'VB'
        || extension[extension.length-1] == 'bat' || extension[extension.length-1] == 'BAT'  || extension[extension.length-1] == 'ini' || extension[extension.length-1] == 'INIT' ){
        
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                 
         $alert({title:'เกิดข้อผิดพลาด', content:'ไม่อนุญาติให้อัพโหลดไฟล์นามสกุลดังกล่าว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }
            else{
                  $scope.new_file.push(args.file);

            }
          


            // $rootScope.manage_research_fix_this_research.file_name = $scope.new_file[0].name;
         

        });
    });

});



app.controller('fix_lab_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                               $scope.new_file = [];
                               $scope.disabled_search = false;

}
            
         
    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                     $scope.new_file = [];
                        $scope.disabled_search = false;

    $scope.still_not_complete = function(){

        if(!$rootScope.manage_lab_fix_this_lab){
            return true;
        }
        if(!$rootScope.manage_lab_fix_this_lab.name || !$rootScope.manage_lab_fix_this_lab.room ){
            return true;
        }



        if($rootScope.manage_lab_fix_this_lab_init.length == 0){
            return true;
        }

     
    }


   
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }



     $scope.save_to_server = function(my_modal){
 

        // var index;
        // for(index =0;index<$rootScope.manage_lab_all_personnels_in_curri.length;index++){
        //     if($rootScope.manage_lab_fix_this_lab_init.indexOf($rootScope.manage_lab_all_personnels_in_curri[index].personnel_id) != -1){
        //         $rootScope.manage_lab_fix_this_lab.officer.push($rootScope.manage_lab_all_personnels_in_curri[index]);
        //     }
        // }
        $rootScope.manage_lab_fix_this_lab.officer = $rootScope.manage_lab_fix_this_lab_init;
      console.log("save_to_server lab_controller");
        console.log($rootScope.manage_lab_fix_this_lab);
        $http.put(
             '/api/lablist/edit',
             JSON.stringify($rootScope.manage_lab_fix_this_lab),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.manage_lab_still_same();
            $rootScope.manage_lab_my_world_wide_labs = data;
                 $scope.close_modal(my_modal);
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



app.controller('create_lab_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
    $scope.my_new_lab = {};
    $scope.my_new_lab.name = "";
    $scope.my_new_lab.room = "";
    $scope.my_new_lab.officer = [];
              
}

   
    $scope.my_new_lab = {};
    $scope.my_new_lab.name = "";
    $scope.my_new_lab.room = "";
    $scope.my_new_lab.officer = [];
              
    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    $scope.still_not_complete = function(){

        if(! $scope.my_new_lab){
            return true;
        }
        if(! $scope.my_new_lab.name || ! $scope.my_new_lab.room ){
            return true;
        }



        if( $scope.my_new_lab.officer.length == 0){
            return true;
        }

     
    }

    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
 


     $scope.save_to_server = function(my_modal){
        console.log("save_to_server");
        console.log($scope.my_new_lab);
        $scope.my_new_lab.curri_id = $rootScope.manage_lab_curri_id ;
        $scope.my_new_lab.aca_year = $rootScope.manage_lab_aca_year;
    
        $http.post(
             '/api/lablist/newlablist',
             JSON.stringify($scope.my_new_lab),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.manage_lab_still_same();
            $rootScope.manage_lab_my_world_wide_labs =data;
                 $scope.close_modal(my_modal);
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
app.controller('import_to_curri_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
         $scope.nothing_change = true;
                $rootScope.manage_research_my_research_now = {};
                $scope.choose_people = [];
             $scope.result= [];
          
}
 $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
         $scope.nothing_change = true;
                $rootScope.manage_research_my_research_now = {};
                $scope.choose_people = [];


  $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.still_not_complete = function(){

        if(!$scope.curri_choosen || !$scope.choose_people){
          
            return true;
        }

        if($scope.choose_people.length ==0){
           
            return true;
        }

        return false;
    }
    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

     $scope.find_information = function(){

      
        console.log($scope.curri_choosen.curri_id);

        $http.post(
             'api/personnel/getalltnameandid',
             JSON.stringify($scope.curri_choosen.curri_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
               $scope.choose_people = [];
              $scope.result = angular.copy(data);
              $scope.result_to_del = angular.copy(data);
        
              var index;    
              for(index=0;index<$scope.result.length;index++){
                if($rootScope.all_id_we_have_now_in_curri.indexOf($scope.result[index].user_id)!=-1 ){
              
                    $scope.result_to_del.splice($scope.result_to_del.indexOf($scope.result[index]));
                }
              }

              $scope.result = $scope.result_to_del;

             $scope.choose_not_complete = false;
              
            
    
         });

}
 $scope.save_to_server = function(my_modal){
    console.log("save_to_server");
     
        $scope.to_sent = {};
        $scope.to_sent.these_people = $scope.choose_people;
        $scope.to_sent.curri_id = $rootScope.manage_bind_curri_id_now;
           console.log($scope.to_sent);
        $http.post(
             '/api/personnelcurriculum',
             JSON.stringify($scope.to_sent),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.manage_bind_still_same();
            $rootScope.manage_bind_all_people_in_curri = data;
                   $scope.close_modal(my_modal);
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
app.controller('manage_bind_person_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
         $scope.nothing_change = true;
           
    $scope.result = {};
      $scope.all_curri_that_have_privileges = [];
      $rootScope.curri_that_be_president_in($scope.all_curri_that_have_privileges);

}

$rootScope.manage_bind_still_same = function(){
    $scope.nothing_change = true;
}
  $scope.go_to_import = function(){

    var index;
    $rootScope.manage_bind_curri_id_now = $scope.curri_choosen.curri_id;
    $rootScope.all_curri_except_us = [];
         for(index=0;index<$rootScope.all_curriculums.length;index++){
                    if($scope.curri_choosen.curri_id != $rootScope.all_curriculums[index].curri_id){
                        $rootScope.all_curri_except_us.push($rootScope.all_curriculums[index]);
                    }


                }
    $rootScope.all_curri_except_us.push({curr_tname:"บุคคลที่ไม่มีหลักสูตร",curri_id:999})
    $rootScope.all_id_we_have_now_in_curri = [];
    for(index=0;index< $rootScope.manage_bind_all_people_in_curri.length;index++){
          $rootScope.all_id_we_have_now_in_curri.push($rootScope.manage_bind_all_people_in_curri[index].user_id);
    }


  }


     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
  $scope.nothing_change = true;
   

        $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    


    $scope.remove_person = function(index_to_remove){

         $rootScope.manage_bind_all_people_in_curri.splice(index_to_remove, 1);    
           $scope.nothing_change = false;

    }
    $scope.find_information = function(){

      
        console.log($scope.curri_choosen.curri_id);

        $http.post(
             '/api/personnel/getonlynameandpfname',
             JSON.stringify($scope.curri_choosen.curri_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            
              $rootScope.manage_bind_all_people_in_curri = data;
             $scope.choose_not_complete = false;
               $scope.nothing_change = true;
            
    
         });



    }


    $scope.delete_checked = function(){

      
        var index;

              $scope.delete_these = [];
 
        for(index=0;index < $rootScope.manage_bind_all_people_in_curri.length ; index++){
            if($rootScope.manage_bind_all_people_in_curri[index].delete_me == true){
               $scope.delete_these.push($rootScope.manage_bind_all_people_in_curri[index]);
            }
        }

        if($scope.delete_these.length != 0){
             for(index =0 ;index<  $scope.delete_these.length ; index++){
            $rootScope.manage_bind_all_people_in_curri.splice($rootScope.manage_bind_all_people_in_curri.indexOf($scope.delete_these[index]),1);
        }
         $scope.nothing_change = false;
        }
        else{
                $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาเลือกบุคลากรที่ต้องการนำออก',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
        }
       
    }
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
        console.log("save_to_server");
        console.log($rootScope.manage_bind_all_people_in_curri);
         $scope.to_sent  = {};
        if($rootScope.manage_bind_all_people_in_curri.length == 0 ){
           
         
            $scope.to_sent.curri_id = $scope.curri_choosen.curri_id ;


        }
        else{
             $scope.to_sent.people = $rootScope.manage_bind_all_people_in_curri;
             $scope.to_sent.curri_id = $scope.curri_choosen.curri_id ;
        }


    

        $http.put(
             '/api/personnelcurriculum',
             JSON.stringify($scope.to_sent),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.close_modal(my_modal);
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



app.controller('change_priviledge_by_type_president_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;

              $scope.curri_choosen = {}
    $scope.not_choose_title_yet = true;
     
   $scope.manage_privilege_president_result={};
$scope.title_choosen = {};

 $scope.all_curri_that_have_privileges = [];
      $rootScope.curri_that_be_president_in($scope.all_curri_that_have_privileges);


  $http.get('/api/title').success(function (data) {
          
             $scope.all_title = data;
          
           });
}

     $http.get('/api/title').success(function (data) {
          
             $scope.all_title = data;
          
           });
    $scope.not_choose_title_yet = true;
$scope.title_choosen = {};
  
     $scope.choose_not_complete = true;
      
              $scope.curri_choosen = {};
  
                 $scope.manage_privilege_president_result = {};
  

      $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    

    $scope.still_not_complete = function(){
        var index;
        if(!$scope.manage_privilege_president_result){
            return true;
        }
        for(index=0;index<$scope.manage_privilege_president_result.list.length;index++){
            if(!$scope.manage_privilege_president_result.list[index].my_privilege ){
                return true;
            }
        }

           if(angular.equals($scope.copy_save,$scope.manage_privilege_president_result.list)==true){
            return true;
        }
        return false;
    }
    $scope.choose_curri = function(){

          $scope.not_choose_title_yet = true;
      $scope.choose_not_complete = false;

    }

      $scope.find_information = function(){
  

      $scope.to_sent = {};
      $scope.to_sent.curri_id = $scope.curri_choosen.curri_id;
      $scope.to_sent.title_code = $scope.title_choosen.title_code;
     $scope.to_sent.name = $scope.title_choosen.name;
        $http.post(
             '/api/extraprivilegebytype',
             JSON.stringify($scope.to_sent),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            console.log('what we received')
            console.log(data)
                   $scope.not_choose_title_yet = false;
              $scope.manage_privilege_president_result = data;
             $scope.choose_not_complete = false;
               $scope.nothing_change = true;
                     var index;
            var index2;
           
            for(index=0;index< $scope.manage_privilege_president_result.list.length;index++){
                  console.log(index)
                for(index2=0;index2< $scope.manage_privilege_president_result.choices.length;index2++){
                           console.log(index2)
                   
                    if($scope.manage_privilege_president_result.list[index].my_privilege.title_privilege_code == $scope.manage_privilege_president_result.choices[index2].title_privilege_code){
                        $scope.manage_privilege_president_result.list[index].my_privilege = $scope.manage_privilege_president_result.choices[index2];
                   
                    }
                }
            }
    

   $scope.copy_save = angular.copy($scope.manage_privilege_president_result.list);
         });



    }

    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){

        $http.put(
             '/api/extraprivilegebytype',
             JSON.stringify($scope.manage_privilege_president_result),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.close_modal(my_modal);
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



app.controller('create_new_education_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,AUTH_EVENTS, AuthService) {
  
$scope.create_not_complete = function(){

    if(!$scope.new_grad){
        return true;
    }
    if( !$scope.new_grad.major || !$scope.new_grad.college || !$scope.new_grad.grad_year || !$scope.new_grad.pre_major || !$scope.new_grad.degree ){
        return true;
    }

    if(isNaN($scope.new_grad.degree)==true){
        return true;
    }
    if(angular.isNumber($scope.new_grad.grad_year) == false){
        return true;
    }


    if($scope.new_grad.grad_year<=0){
        return true;
    }

    return false;

}

$scope.init = function(){
    $scope.new_grad = {};
    $scope.new_grad.major = "";
    $scope.new_grad.college = "";
    $scope.new_grad.grad_year = "";
    $scope.new_grad.pre_major = "";
    $scope.new_grad.degree = "";
}   



  $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
      $scope.init();
    });

 $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }


$scope.save_to_server = function(my_modal){
    $scope.new_grad.personnel_id = $rootScope.current_user.user_id;
     $http.post(
             '/api/education',
             JSON.stringify($scope.new_grad),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.current_user.information.education = data;
            $rootScope.save_obj.information.education = data;
               $scope.close_modal(my_modal);
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


app.controller('fix_education_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,AUTH_EVENTS, AuthService) {
  $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

$scope.create_not_complete = function(){

    if(!$scope.fix_this_edu){
        return true;
    }
    if( !$scope.fix_this_edu.major || !$scope.fix_this_edu.college || !$scope.fix_this_edu.grad_year || !$scope.fix_this_edu.pre_major || !$scope.fix_this_edu.degree ){
        return true;
    }

    if(isNaN($scope.fix_this_edu.degree)==true){
        return true;
    }
    if(angular.isNumber($scope.fix_this_edu.grad_year) == false){
        return true;
    }


    if($scope.fix_this_edu.grad_year<=0){
        return true;
    }

    return false;

}

$scope.init = function(){

    $scope.fix_this_edu = $rootScope.manage_profile_fix_this_edu;
}


  $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

$scope.save_to_server = function(my_modal){

     $http.put(
             '/api/education',
             JSON.stringify($scope.fix_this_edu),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.current_user.information.education = data;
            $rootScope.save_obj.information.education =data;
               $scope.close_modal(my_modal);
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


app.controller('manage_profile_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,AUTH_EVENTS, AuthService) {

$scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

$scope.back_to_default = function(){
       angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
       $scope.files= [];

}
$scope.fix_not_complete = function(){
    if(!$rootScope.current_user ){
        console.log("1")
        return true;
    }
    if(!$rootScope.current_user.information.e_name || !$rootScope.current_user.information.t_name || !$rootScope.current_user.information.addr || !$rootScope.current_user.information.tel || !$rootScope.current_user.information.email){
              
        return true;
    }

  
        // console.log($rootScope.current_user.information.e_prename);
        // console.log();
        if($rootScope.current_user.information.e_prename.length == 14 && $rootScope.current_user.information.e_prename  != 'Assoc.Prof.Dr.'){
               return true;
        }
     
              if($rootScope.current_user.information.t_prename.length == 14 ){
               return true;
        } 


             if($rootScope.current_user.information.status.length== 14 ){
               return true;
        }       
 

    if($rootScope.current_user.information.tel.length <9){
              console.log("4")
        return true;
    }
    return false;
}

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

 $scope.init = function(){
     $scope.files = [];
     $scope.please_wait = false;
     $scope.nothing_change = true;
     $scope.save_interest = angular.copy($rootScope.current_user.information.interest)

     $rootScope.save_obj = angular.copy($rootScope.current_user);
         
 }

$scope.nothing_really_change = function(){


    var equal = true;
    var dont_go = false;
    var index;
    var index2;

    if (!$scope.save_interest || !$rootScope.current_user.information.interest){
        return true;
    }
    if($rootScope.current_user.information.interest.length != $scope.save_interest.length){
         equal = false;
    }
    else{
          for(index =0 ;index<$rootScope.current_user.information.interest.length;index++ ){
         
               
                if($rootScope.current_user.information.interest[index] != $scope.save_interest[index]){
             
                    equal = false;
                 
                    break;
                }

        
           
          }
    }
  
    if(equal == true){
  
        return $scope.nothing_change;


    }
    else{
         
        return false;
    }

}
$scope.same_interest = function(){
    console.log($rootScope.current_user.information.interest == $scope.save_interest)
    return $rootScope.current_user.information.interest == $scope.save_interest;
}
 $scope.something_change = function(){
    $scope.nothing_change = false;
 }
 $scope.$on("fileSelected", function (event, args) {
        $scope.$apply(function () {            
           var extension = args.file.name.split('.');

 if(args.file.size > 25000000){
                   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                $alert({title:'เกิดข้อผิดพลาด', content:'ไฟล์ที่เลือกมีขนาดมากกว่า 25 MB',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
            else        if(extension[extension.length-1] == 'exe' || extension[extension.length-1] == 'EXE' || extension[extension.length-1] == 'vb' || extension[extension.length-1] == 'VB'
        || extension[extension.length-1] == 'bat' || extension[extension.length-1] == 'BAT'  || extension[extension.length-1] == 'ini' || extension[extension.length-1] == 'INIT' ){
        
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                 
         $alert({title:'เกิดข้อผิดพลาด', content:'ไม่อนุญาติให้อัพโหลดไฟล์นามสกุลดังกล่าว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }
              else        if(extension[extension.length-1] != 'jpeg' && extension[extension.length-1]!='jpg' && extension[extension.length-1] != 'png' && extension[extension.length-1] != 'bmp'
        && extension[extension.length-1] != 'JPEG' && extension[extension.length-1] !='JPG'  && extension[extension.length-1] != 'PNG' && extension[extension.length-1] != 'BMP' ){
        
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                 
         $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาอัพโหลดไฟล์รูปภาพสกุล .jpg, .jpeg, .png, .bmp เท่านั้น',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }

            else{
                $scope.files = [];
                   $scope.files.push(args.file);
                   $scope.nothing_change = false;
            }

         
        });
    });

$scope.go_to_fix = function(fix_this_obj){
    $rootScope.manage_profile_fix_this_edu = angular.copy(fix_this_obj);
}

$scope.remove_education = function(index_to_remove){
    $rootScope.current_user.information.education.splice(index_to_remove,1);
    $scope.nothing_change = false;
}

 $scope.close_modal = function(my_modal){
     $scope.init();
        my_modal.$hide();
    }

    $scope.back_close_modal = function(my_modal){
        $rootScope.current_user = $rootScope.save_obj;
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal) {
   $scope.please_wait = true;
     
      var formData = new FormData();
      if($scope.files.length != 0){
            $rootScope.current_user.information.file_name_pic = $scope.files[0].name;
      }

    formData.append("model", angular.toJson($rootScope.current_user));
formData.append("file" , $scope.files[0]);
   
        $http({
            method: 'PUT',
            url: "/api/users/edit",

            headers: { 'Content-Type': undefined },


            data:formData,
            transformRequest: angular.indentity 

        }).
        success(function (data, status, headers, config) {
    $rootScope.current_user = data;
             
 $scope.close_modal(my_modal);
                $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

               
           
        }).
        error(function (data, status, headers, config) {
                      $scope.please_wait = false;
            $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
    }




 });
app.controller('login_controller', function($scope, $http,$alert,$loading,$timeout,$rootScope,ngDialog,request_all_curriculums_service_server,request_years_from_curri_choosen_service,AUTH_EVENTS, AuthService) {
    $scope.credentials = {
        username: '',
        password: ''
      };

       $scope.close_modal = function(my_modal){
     
        my_modal.$hide();
    }
      $scope.login = function (my_modal) {
  //        var user = AuthService.login($scope.credentials);
  //        console.log(user);
  //        $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
  //         $scope.setcurrent_user(user);
  // $scope.$parent.already_login = true;
  $scope.credentials.username.toLowerCase();
        AuthService.login($scope.credentials).then(function (user) {
            $rootScope.have_privilege_in_these_curri = {};
          $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);  
          $scope.setcurrent_user(user);
             my_modal.$hide();

               $alert({title:'เข้าสู่ระบบสำเร็จ', content:'ยินดีต้อนรับ '+$rootScope.current_user.username,alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
   if(!!$rootScope.current_user.not_send_primary){
         
              $rootScope.open_modal_primary_not_send();
         }
        

        }, function () {


          $rootScope.$broadcast(AUTH_EVENTS.loginFailed);

            $alert({title:'เกิดข้อผิดพลาด', content:'ชื่อผู้ใช้และรหัสผ่านไม่ถูกต้อง',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});

        });


      };


   
});


app.controller('change_priviledge_person_president_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;

              $scope.curri_choosen = {}
    $scope.not_choose_title_yet = true;
     
   $scope.manage_privilege_president_result={};
$scope.title_choosen = {};
    $scope.all_curri_that_have_privileges = [];
      $rootScope.curri_that_be_president_in($scope.all_curri_that_have_privileges);

  $http.get('/api/title').success(function (data) {
          
             $scope.all_title = data;
          
           });
}

     $http.get('/api/title').success(function (data) {
          
             $scope.all_title = data;
          
           });
    $scope.not_choose_title_yet = true;
$scope.title_choosen = {};
  
     $scope.choose_not_complete = true;
      
              $scope.curri_choosen = {};
  
                 $scope.manage_privilege_president_result = {};
  

      $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    

    $scope.still_not_complete = function(){
        var index;
        if(!$scope.manage_privilege_president_person_result){
            return true;
        }
        for(index=0;index<$scope.manage_privilege_president_person_result.list.length;index++){
            if(!$scope.manage_privilege_president_person_result.list[index].my_privilege ){
                return true;
            }
        }

           if(angular.equals($scope.copy_save,$scope.manage_privilege_president_person_result.list)==true){
            return true;
        }
        return false;
    }
    $scope.choose_curri = function(){

          $scope.not_choose_title_yet = true;
      $scope.choose_not_complete = false;

    }

      $scope.find_information = function(){
  

      $scope.to_sent = {};
      $scope.to_sent.curri_id = $scope.curri_choosen.curri_id;
      $scope.to_sent.title_code = $scope.title_choosen.title_code;
     $scope.to_sent.name = $scope.title_choosen.name;
        $http.post(
             '/api/extraprivilege',
             JSON.stringify($scope.to_sent),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {

                   $scope.not_choose_title_yet = false;
              $scope.manage_privilege_president_person_result = data;
             $scope.choose_not_complete = false;
               $scope.nothing_change = true;


                     var index;
            var index2;
           
            for(index=0;index< $scope.manage_privilege_president_person_result.list.length;index++){
                  console.log(index)
                for(index2=0;index2< $scope.manage_privilege_president_person_result.choices.length;index2++){
                           console.log(index2)
                   
                    if($scope.manage_privilege_president_person_result.list[index].my_privilege.title_privilege_code == $scope.manage_privilege_president_person_result.choices[index2].title_privilege_code){
                        $scope.manage_privilege_president_person_result.list[index].my_privilege = $scope.manage_privilege_president_person_result.choices[index2];
                   
                    }
                }
            }


   $scope.copy_save = angular.copy($scope.manage_privilege_president_person_result.list);
         });



    }

    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){

        $http.put(
             '/api/extraprivilege',
             JSON.stringify($scope.manage_privilege_president_person_result),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.close_modal(my_modal);
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



app.controller('change_priviledge_by_type_admin_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
    console.log('heellll');
    $scope.not_choose_title_yet = true;
     
   $scope.manage_privilege_admin_result={};
$scope.title_choosen = {};

  $http.get('/api/title').success(function (data) {
          
             $scope.all_title = data;
          
           });
}

     $http.get('/api/title').success(function (data) {
          
             $scope.all_title = data;
          
           });
    $scope.not_choose_title_yet = true;
$scope.title_choosen = {};
  
  
      
  
                 $scope.manage_privilege_admin_result = {};
  

      $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    

    $scope.still_not_complete = function(){
        var index;
        if(!$scope.manage_privilege_admin_result){
            return true;
        }
        for(index=0;index<$scope.manage_privilege_admin_result.list.length;index++){
            if(!$scope.manage_privilege_admin_result.list[index].my_privilege ){
                return true;
            }
        }

        if(angular.equals($scope.copy_save,$scope.manage_privilege_admin_result.list)==true){

            return true;
        }

        return false;
    }
    $scope.choose_curri = function(){

          $scope.not_choose_title_yet = true;
      $scope.choose_not_complete = false;

    }

      $scope.find_information = function(){
  
      $scope.to_sent = {};
     
      $scope.to_sent.title_code = $scope.title_choosen.title_code;
     $scope.to_sent.name = $scope.title_choosen.name;

        $http.post(
             '/api/defaultprivilegebytype',
             JSON.stringify($scope.to_sent),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {

                   $scope.not_choose_title_yet = false;
              $scope.manage_privilege_admin_result = data;
           
               $scope.nothing_change = true;
               console.log('success ni');
               var index;
               var index2;
                for(index=0;index< $scope.manage_privilege_admin_result.list.length;index++){
                 
                for(index2=0;index2< $scope.manage_privilege_admin_result.choices.length;index2++){
                          
                   
                    if($scope.manage_privilege_admin_result.list[index].my_privilege.title_privilege_code == $scope.manage_privilege_admin_result.choices[index2].title_privilege_code){
                        $scope.manage_privilege_admin_result.list[index].my_privilege = $scope.manage_privilege_admin_result.choices[index2];
                 
                    }
                }
            }

            $scope.copy_save = angular.copy($scope.manage_privilege_admin_result.list);
    
         });



    }

    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){

        $http.put(
             '/api/defaultprivilegebytype',
             JSON.stringify($scope.manage_privilege_admin_result),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.close_modal(my_modal);
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
app.controller('manage_research_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
         $scope.nothing_change = true;
                $rootScope.manage_research_my_research_now = {};
                 $scope.all_curri_that_have_privileges = [];
  $scope.$parent.scan_only_privilege_curri('9',$scope.all_curri_that_have_privileges);
}

$rootScope.manage_research_still_same = function(){
      $scope.nothing_change = true;
}

  
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
  $scope.nothing_change = true;

      $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    
    $scope.download_research = function(path_research){
        $scope.download_file(path_research);
    }

    $scope.download_file = function(path) { 
        window.open(path, '_blank', "");  
    }

    $scope.go_to_fix_research = function(this_research){

          $http.post(
             '/api/teacher/getname',
             JSON.stringify($scope.curri_choosen.curri_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.manage_research_all_teachers_in_curri = data;

     
        $rootScope.manage_research_fix_this_research = angular.copy(this_research);
        // console.log( $rootScope.manage_research_fix_this_research);
  
        //     console.log( $rootScope.manage_research_all_teachers_in_curri);

            // $rootScope.manage_lab_research_this_research_init = [];
            // console.log("$rootScope.manage_research_fix_this_research");
            // console.log($rootScope.manage_research_fix_this_research);
            //  var index;
            //  var 
            //  for(index = 0; index<$rootScope.manage_research_fix_this_research.researcher.length;index++ ){
            //     $rootScope.manage_lab_research_this_research_init.push($rootScope.manage_research_fix_this_research.researcher[index].teacher_id);
            //  }

            //  console.log("manage_lab_research_this_research_init");
            //  console.log($rootScope.manage_lab_research_this_research_init);




                var index;
        var inside_index;
        $rootScope.manage_lab_research_this_research_init = [];

        for(index =0;index<$rootScope.manage_research_fix_this_research.researcher.length;index++){
            for(inside_index=0;inside_index<$rootScope.manage_research_all_teachers_in_curri.length;inside_index++){
                if($rootScope.manage_research_all_teachers_in_curri[inside_index].teacher_id  == $rootScope.manage_research_fix_this_research.researcher[index].teacher_id){
                          $rootScope.manage_lab_research_this_research_init.push($rootScope.manage_research_all_teachers_in_curri[inside_index]);
                }
          
            }
        }

         });




    }   

    $scope.go_to_create_research =function(){
            $rootScope.manage_reseach_my_curri_id_now = $scope.curri_choosen.curri_id;
           $http.post(
             '/api/teacher/getname',
             JSON.stringify($scope.curri_choosen.curri_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.manage_research_all_teachers_in_curri = data;
         });
           
    }


    $scope.remove_research = function(index_research_to_remove){

         $rootScope.manage_research_my_research_now.splice(index_research_to_remove, 1);    
           $scope.nothing_change = false;

    }
    $scope.find_information = function(){

      
        console.log($scope.curri_choosen.curri_id);

        $http.post(
             '/api/research/getresearch',
             JSON.stringify($scope.curri_choosen.curri_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            
              $rootScope.manage_research_my_research_now = data;
             $scope.choose_not_complete = false;
               $scope.nothing_change = true;
            
    
         });

//  $rootScope.manage_research_my_research_now  =[{"researcher":[{"teacher_id":"00009","t_name":"รศ.ดร.ศุภมิตร จิตตะยโศธร"}],"research_id":1,"name":"An Extended NIAM Conceptual Schema Model for Object Databases","curri_id":"21","file_name":"download/research/file1.pdf","year_publish":2002},{"researcher":[{"teacher_id":"00009","t_name":"รศ.ดร.ศุภมิตร จิตตะยโศธร"}],"research_id":2,"name":"A temporal object relational SQL language with attribute timestamping in a temporal transparency environment","curri_id":"21","file_name":"download/research/file2.pdf","year_publish":2008},{"researcher":[{"teacher_id":"00007","t_name":"อาจารย์บัณฑิต พัสยา"},{"teacher_id":"00009","t_name":"รศ.ดร.ศุภมิตร จิตตะยโศธร"}],"research_id":3,"name":"A temporal object oriented conceptual schema model","curri_id":"21","file_name":"download/research/file3.pdf","year_publish":2001},{"researcher":[{"teacher_id":"00009","t_name":"รศ.ดร.ศุภมิตร จิตตะยโศธร"}],"research_id":4,"name":"The design and implementation of an ORM-based information warehouse","curri_id":"21","file_name":"download/research/file4.pdf","year_publish":2006},{"researcher":[{"teacher_id":"00007","t_name":"อาจารย์บัณฑิต พัสยา"}],"research_id":5,"name":"Feature Extraction from Retinal Fundus Image for Early Detection of Diabetic Retinopathy","curri_id":"21","file_name":"download/research/file5.pdf","year_publish":2013},{"researcher":[{"teacher_id":"00003","t_name":"รศ.ดร.อรฉัตร จิตต์โสภักตร์"}],"research_id":6,"name":"An Improvement of PDLZW implementation with a Modified WSC Updating Technique on FPGA","curri_id":"21","file_name":"download/research/file6.pdf","year_publish":2009},{"researcher":[{"teacher_id":"00003","t_name":"รศ.ดร.อรฉัตร จิตต์โสภักตร์"},{"teacher_id":"00013","t_name":"ดร.อำนาจ ขาวเน"}],"research_id":7,"name":"A Study of using L1-norm with Image Watermarking on SVD Domain","curri_id":"21","file_name":"download/research/file7.pdf","year_publish":2007},{"researcher":[{"teacher_id":"00006","t_name":"รศ.ดร.บุญธีร์ เครือตราชู"}],"research_id":8,"name":"Static task scheduling and grain packing in parallel-processing systems","curri_id":"21","file_name":"download/research/file8.pdf","year_publish":1986}]
// ;
// $scope.choose_not_complete = false;

    }
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
        console.log("save_to_server");
        console.log($rootScope.manage_research_my_research_now);

        if($rootScope.manage_research_my_research_now.length == 0 ){
            $scope.to_sent  = {};
            $scope.to_sent.research_id = -1;
            $scope.to_sent.curri_id = $scope.curri_choosen.curri_id ;
            $rootScope.manage_research_my_research_now.push($scope.to_sent);

        }
        $http.put(
             '/api/research/delete',
             JSON.stringify($rootScope.manage_research_my_research_now),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.close_modal(my_modal);
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

app.controller('add_committee_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {

              
                $scope.choose_people = [];

   $http.post(
             '/api/committee/getnoncommittee',
             JSON.stringify({'aca_year':$rootScope.manage_committee_who_aca_year_now,'curri_id':$rootScope.manage_committee_who_curri_id_now,'these_people':$rootScope.manage_committee_who_all_committees}),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.people_in_curri = data;
    
         });
          
}

    
                console.log('start dad')
      
     


  $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.still_not_complete = function(){

        if(!$scope.choose_people){
          
            return true;
        }

        if($scope.choose_people.length ==0){
           
            return true;
        }

        return false;
    }
    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {

              $scope.init();
    });

    
 $scope.save_to_server = function(my_modal){
    console.log("save_to_server");
     
        $scope.to_sent = {};
        $scope.to_sent.these_people = $scope.choose_people;
        $scope.to_sent.curri_id = $rootScope.manage_committee_who_curri_id_now;
$scope.to_sent.aca_year = $rootScope.manage_committee_who_aca_year_now;
           console.log($scope.to_sent);
        $http.post(
             '/api/committee/new',
             JSON.stringify($scope.to_sent),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.manage_committee_who_all_committees = data;
                   $scope.close_modal(my_modal);
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

app.controller('manage_committee_who_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
   
              $scope.year_choosen = {};
              $scope.curri_choosen = {}
                   $scope.corresponding_aca_years = [];
         $scope.nothing_change = true;
            $scope.result = {};
      $scope.all_curri_that_have_privileges = [];
      $rootScope.curri_that_be_president_in($scope.all_curri_that_have_privileges);
}


  
     $scope.choose_not_complete = true;
   
               $scope.year_choosen = {};
              $scope.curri_choosen = {}
             
                    $scope.result = {};
  $scope.nothing_change = true;

      $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

      $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
         $scope.nothing_change = true;
    
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen,999).then(function(data) {
$scope.corresponding_indicators = [];
            $scope.corresponding_aca_years = data;


          });


    }

    $scope.go_to_add_committee =function(){
          $rootScope.manage_committee_who_curri_id_now = $scope.curri_choosen.curri_id;
           $rootScope.manage_committee_who_aca_year_now = $scope.year_choosen.aca_year ;
     

    }



    $scope.remove_committee = function(index_committee_to_remove){

         $rootScope.manage_committee_who_all_committees.splice(index_committee_to_remove, 1);    
           $scope.nothing_change = false;

    }
    $scope.find_information = function(){

      
        console.log($scope.year_choosen);

        $http.post(
             '/api/committee/getcommittee',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            
              $rootScope.manage_committee_who_all_committees = data;
             $scope.choose_not_complete = false;
               $scope.nothing_change = true;
            
    
         });

    }
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
        console.log("save_to_server");

        $scope.to_sent = {};
    
       
           $scope.to_sent.aca_year = $scope.year_choosen.aca_year ;
            $scope.to_sent.curri_id = $scope.curri_choosen.curri_id ;
            $scope.to_sent.these_people =     $rootScope.manage_committee_who_all_committees;
     
        $http.put(
             '/api/committee/',
             JSON.stringify($scope.to_sent),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.close_modal(my_modal);
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


app.controller('show_education_personnel_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,Lightbox) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
         $scope.nothing_change = true;
                $rootScope.manage_research_my_research_now = {};
                 $scope.all_curri_that_have_privileges = [];
  $scope.$parent.scan_only_privilege_curri('7',$scope.all_curri_that_have_privileges);
}


  
    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });


     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
  $scope.nothing_change = true;

   $scope.openLightboxModal = function (pic) {
    $scope.fake_array = [];
    $scope.fake_array.push(pic.file_name_pic);
    Lightbox.openModal($scope.fake_array, 0);
  };


    $scope.download_research = function(path_research){
        $scope.download_file(path_research);
    }

    $scope.download_file = function(path) { 
        window.open(path, '_blank', "");  
    }

    $scope.close_modal = function(my_modal){
             $scope.choose_not_complete = true;
        $scope.curri_choosen = {};
        my_modal.$hide();
    }

    $scope.find_information = function(){

      
        console.log($scope.curri_choosen.curri_id);

        $http.post(
             '/api/personnel/getwitheducation',
             JSON.stringify($scope.curri_choosen.curri_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            console.log(data);
              $scope.result =data;
             $scope.choose_not_complete = false;
              
            
    
         });

}
});



// app.controller('manage_minutes_minute_attendees_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {

// });


app.controller('create_minute_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
              
                    $scope.result = {};
                   $scope.my_new_minute = {};
                      $scope.please_wait = false;

$scope.my_pictures.flow.files = [];

  $scope.my_new_minute.date = "";
  $scope.my_new_minute.attendee =[];
  $scope.my_new_minute.topic_name = "";
  $scope.my_file = [];
 
  $scope.show_gallery = false;

   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
}

    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });


 $scope.show_gallery = false;
  $scope.choose_not_complete = true;
         $scope.year_choosen = {};
            $scope.please_wait = false;
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                   $scope.my_new_minute = {};

  $scope.my_new_minute.date = "";
  $scope.my_new_minute.attendee =[];
  $scope.my_new_minute.topic_name = "";
  $scope.my_file = [];
$scope.my_pictures = {};
         $scope.my_pictures.flow={}; 
         console.log("outside init");         
$scope.my_pictures.flow.files = [];
    $scope.$on("fileSelected", function (event, args) {

     
        var extension = args.file.name.split('.');


        $scope.$apply(function () {            
            $scope.my_file = [];
            //add the file object to the scope's files collection

             if(args.file.size > 25000000){
                   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                $alert({title:'เกิดข้อผิดพลาด', content:'ไฟล์ที่เลือกมีขนาดมากกว่า 25 MB',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
            else        if(extension[extension.length-1] == 'exe' || extension[extension.length-1] == 'EXE' || extension[extension.length-1] == 'vb' || extension[extension.length-1] == 'VB'
        || extension[extension.length-1] == 'bat' || extension[extension.length-1] == 'BAT'  || extension[extension.length-1] == 'ini' || extension[extension.length-1] == 'INIT' ){
        
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                 
         $alert({title:'เกิดข้อผิดพลาด', content:'ไม่อนุญาติให้อัพโหลดไฟล์นามสกุลดังกล่าว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }
            else{
                $scope.my_file.push(args.file);
            }
            
        });
    });


// $scope.check_everything = function(this_thing){
//     console.log('check_everything');
//    console.log(this_thing);
//    console.log($scope.my_pictures.flow);
// }
     $scope.still_not_complete = function(){
        if(!$scope.my_pictures.flow){
            return true;
        }
        if( $scope.my_pictures.flow.files.length ==0 ||! $scope.my_new_minute.topic_name || $scope.my_file.length ==0 || !$scope.my_new_minute.date || $scope.my_new_minute.attendee.length ==0 ){
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

$scope.show_my_pictures=function(){

     $scope.show_gallery = true;
     var index;

        $scope.to_del = [];

     for(index=0;index<$scope.my_pictures.flow.files.length;index++){
        console.log("loop");
        console.log($scope.my_pictures.flow.files[index].name);
        console.log($scope.my_pictures.flow.files[index].size);
        if ($scope.my_pictures.flow.files[index].size > 2000000){
            console.log("remove file");
            console.log($scope.my_pictures.flow.files[index].name);
             $scope.to_del.push($scope.my_pictures.flow.files[index]);

        }
    
     }
    

    for(index=0;index<$scope.to_del.length;index++){
       $scope.my_pictures.flow.files.splice( $scope.my_pictures.flow.files.indexOf($scope.to_del[index]),1);
    }

}
    $scope.save_to_server = function(my_modal) {
           $scope.please_wait = true;
 $scope.my_new_minute.curri_id = $rootScope.manage_minutes_curri_id;
 $scope.my_new_minute.aca_year = $rootScope.manage_minutes_aca_year;
 $scope.my_new_minute.teacher_id =  $rootScope.current_user.user_id;
  $scope.my_new_minute.file_name =  $scope.my_file[0].name;
  $scope.my_new_minute.pictures =  [];
      var formData = new FormData();


  
        var index = 0;
        for (index = 0 ;index< $scope.my_pictures.flow.files.length;index++){
                $scope.my_obj = {};
                $scope.my_obj.minutes_id = 0;
                $scope.my_obj.file_name = $scope.my_pictures.flow.files[index].file.name;
              $scope.my_new_minute.pictures.push($scope.my_obj);
            formData.append("picture"+(index+1), $scope.my_pictures.flow.files[index].file );
        }
        formData.append("model", angular.toJson( $scope.my_new_minute));

      formData.append("file", $scope.my_file[0] );

        $http({
            method: 'POST',
            url: "/api/minutes/add",

            headers: { 'Content-Type': undefined },


            data:formData,
            transformRequest: angular.indentity 

        }).
        success(function (data, status, headers, config) {
        $rootScope.manage_minutes_still_same();
                $rootScope.manage_minutes_my_world_wide_minutes =data;
                $scope.close_modal(my_modal);
                $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

              
           
        }).
        error(function (data, status, headers, config) {
              $scope.please_wait = false;
            $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
    }


});


app.controller('change_password_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {

       $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

  $scope.init = function(){
    $scope.old_password = '';
        $scope.new_password = '';
            $scope.new_password2 = '';
            $scope.show_error =false;
            $scope.error_msg = '';
  }


 $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

  $scope.still_not_complete = function(){
    if(!$scope.old_password || !$scope.new_password || !$scope.new_password2){
        return true;
    }
    if($scope.new_password != $scope.new_password2){
           $scope.error_msg = '*** กรุณากรอกรหัสผ่านใหม่ให้สอดคล้องกัน';
        $scope.show_error =true;
        return true;
    }
    if($scope.new_password2.length < 8)

    {
                  $scope.error_msg = '*** กรุณากรอกรหัสผ่านใหม่ที่มีความยาวมากกว่า 8 อักขระ';
                $scope.show_error =true;
        return true;
    }
       $scope.show_error =false;
    return false;
  }

  $scope.save_to_server = function(my_modal){
      console.log("save_to_server");

        $scope.to_sent = {};
       
        $scope.to_sent.user_id =   $rootScope.current_user.user_id;
         $scope.to_sent.old_password = $scope.old_password;
        $scope.to_sent.new_password = $scope.new_password;
        console.log($scope.to_sent);

        $http.put(
             '/api/users/changepassword',
             JSON.stringify($scope.to_sent),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {

                  $scope.close_modal(my_modal);
               $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
         
         })
    .error(function(data, status, headers, config) {
                  if(status==500){

     $alert({title:'เกิดข้อผิดพลาด', content:'รหัสผ่านเก่าไม่ถูกต้อง',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
  }
});


app.controller('change_username_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {

       $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

  $scope.init = function(){
    $scope.new_username = '';
$scope.show_error = false;
$scope.error_msg = '';
  }


 $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

  $scope.still_not_complete = function(){
    if(!$scope.new_username){
        $scope.show_error = true;
        $scope.error_msg = '*** กรุณากรอกชื่อผู้ใช้ใหม่ที่เป็นอักขระภาษาอังกฤษหรือตัวเลขเท่านั้น';
        return true;
    }

    return false;
  }

  $scope.save_to_server = function(my_modal){
      console.log("save_to_server");

        $scope.to_sent = {};
        $scope.to_sent.username = $scope.new_username;
        $scope.to_sent.user_id =   $rootScope.current_user.user_id;
        console.log($scope.to_sent);

        $http.put(
             '/api/users/changeusername',
             JSON.stringify($scope.to_sent),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
              $rootScope.current_user.username = $scope.to_sent.username;
                  $scope.close_modal(my_modal);
               $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});
         
         })
    .error(function(data, status, headers, config) {
                  if(status==500){

     $alert({title:'เกิดข้อผิดพลาด', content:'ชื่อผู้ใช้นี้มีอยู่แล้วในระบบ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
     }

  }); 
  }
});


app.controller('create_album_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {
$scope.init =function() {
   $scope.please_wait = false;
      
             
                   $scope.my_new_album = {};
      $scope.my_new_album.name = "";
$scope.my_pictures.flow.files = [];

 


   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
}


 $scope.show_gallery = false;


                $scope.please_wait = false;
              
     $scope.my_new_album = {};
      $scope.my_new_album.name = "";
$scope.my_pictures = {};
         $scope.my_pictures.flow={}; 
         console.log("outside init");         
$scope.my_pictures.flow.files = [];
    $scope.$on("fileSelected", function (event, args) {

     
        var extension = args.file.name.split('.');


        $scope.$apply(function () {            
            $scope.my_file = [];
            

             if(args.file.size > 25000000){
                   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                $alert({title:'เกิดข้อผิดพลาด', content:'ไฟล์ที่เลือกมีขนาดมากกว่า 25 MB',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
            else        if(extension[extension.length-1] == 'exe' || extension[extension.length-1] == 'EXE' || extension[extension.length-1] == 'vb' || extension[extension.length-1] == 'VB'
        || extension[extension.length-1] == 'bat' || extension[extension.length-1] == 'BAT'  || extension[extension.length-1] == 'ini' || extension[extension.length-1] == 'INIT' ){
        
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                 
         $alert({title:'เกิดข้อผิดพลาด', content:'ไม่อนุญาติให้อัพโหลดไฟล์นามสกุลดังกล่าว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }      else        if(extension[extension.length-1] != 'jpeg' && extension[extension.length-1]!='jpg' && extension[extension.length-1] != 'png' && extension[extension.length-1] != 'bmp'
        && extension[extension.length-1] != 'JPEG' && extension[extension.length-1] !='JPG'  && extension[extension.length-1] != 'PNG' && extension[extension.length-1] != 'BMP' ){
        
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                 
         $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาอัพโหลดไฟล์รูปภาพสกุล .jpg, .jpeg, .png, .bmp เท่านั้น',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }

            else{
                 $scope.my_file.push(args.file);
            }
           
        });
    });


// $scope.check_everything = function(this_thing){
//     console.log('check_everything');
//    console.log(this_thing);
//    console.log($scope.my_pictures.flow);
// }

    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    

     $scope.still_not_complete = function(){
        if(!$scope.my_pictures.flow){
            return true;
        }
        if( $scope.my_pictures.flow.files.length ==0 ||! $scope.my_new_album.name ){
            return true;
        }

        else{
            var index;
            for(index=0;index<$scope.my_pictures.flow.files.length;index++){
                if(!$scope.my_pictures.flow.files[index].caption){
                    return true;
                }
      
            }
                   return false;
        }


       
     }
    
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

$scope.show_my_pictures=function(){
var index;
     $scope.show_gallery = true;
         $scope.to_del = [];

     for(index=0;index<$scope.my_pictures.flow.files.length;index++){
        console.log("loop");
        console.log($scope.my_pictures.flow.files[index].name);
        console.log($scope.my_pictures.flow.files[index].size);
        if ($scope.my_pictures.flow.files[index].size > 2000000){
            console.log("remove file");
            console.log($scope.my_pictures.flow.files[index].name);
             $scope.to_del.push($scope.my_pictures.flow.files[index]);

        }
    
     }
    

    for(index=0;index<$scope.to_del.length;index++){
       $scope.my_pictures.flow.files.splice( $scope.my_pictures.flow.files.indexOf($scope.to_del[index]),1);
    }

}
    $scope.save_to_server = function(my_modal) {
           $scope.please_wait = true;
 $scope.my_new_album.curri_id =    $rootScope.manage_album_curri_id ;
 $scope.my_new_album.aca_year =    $rootScope.manage_album_aca_year;
 $scope.my_new_album.personnel_id =   $rootScope.current_user.user_id;



  $scope.my_new_album.pictures =  [];

      var formData = new FormData();



        var index = 0;
        for (index = 0 ;index< $scope.my_pictures.flow.files.length;index++){
            $scope.my_obj = {};
            $scope.my_obj.caption = $scope.my_pictures.flow.files[index].caption;
            $scope.my_obj.file_name = $scope.my_pictures.flow.files[index].file.name;
            $scope.my_obj.gallery_id = 0;

              $scope.my_new_album.pictures.push($scope.my_obj);
            formData.append("picture"+(index+1), $scope.my_pictures.flow.files[index].file );
        }
        formData.append("model", angular.toJson( $scope.my_new_album));
        console.log("send");
        console.log($scope.my_new_album);

        $http({
            method: 'POST',
            url: "/api/gallery/add",

            headers: { 'Content-Type': undefined },


            data:formData,
            transformRequest: angular.indentity 

        }).
        success(function (data, status, headers, config) {
        $rootScope.manage_album_still_same();
                $rootScope.manage_album_my_world_wide_album =data;
                $scope.close_modal(my_modal);
                $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

              
           
        }).
        error(function (data, status, headers, config) {
              $scope.please_wait = false;
            $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
    }


});

app.controller('fix_minute_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,Lightbox) {
 
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                   $scope.my_new_minute = {};

  $scope.my_new_minute.date = "";
  $scope.my_new_minute.attendee =[];
     $scope.please_wait = false;
  $scope.my_new_minute.topic_name = "";
  $scope.my_file = [];
 $scope.my_pictures.flow.files = [];
  $scope.show_gallery = false;
 $scope.disabled_search = false;
   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
}
   $scope.please_wait = false;
 $scope.show_gallery = false;
  $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                   $scope.my_new_minute = {};

  $scope.my_new_minute.date = "";
  $scope.my_new_minute.attendee =[];
  $scope.my_new_minute.topic_name = "";
  $scope.my_file = [];
$scope.my_pictures = {};
 $scope.disabled_search = false;
    $scope.$on("fileSelected", function (event, args) {

     
        var extension = args.file.name.split('.');


        $scope.$apply(function () {            
            $scope.my_file = [];
            

             if(args.file.size > 25000000){
                   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                $alert({title:'เกิดข้อผิดพลาด', content:'ไฟล์ที่เลือกมีขนาดมากกว่า 25 MB',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
            else        if(extension[extension.length-1] == 'exe' || extension[extension.length-1] == 'EXE' || extension[extension.length-1] == 'vb' || extension[extension.length-1] == 'VB'
        || extension[extension.length-1] == 'bat' || extension[extension.length-1] == 'BAT'  || extension[extension.length-1] == 'ini' || extension[extension.length-1] == 'INIT' ){
        
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                 
         $alert({title:'เกิดข้อผิดพลาด', content:'ไม่อนุญาติให้อัพโหลดไฟล์นามสกุลดังกล่าว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }

            else{
                  $scope.my_file.push(args.file);
            }
          
        });
    });

    $scope.test2 = function(){
        console.log($scope.meme);
        console.log($rootScope.manage_minutes_fix_this_minute.attendee);
     }
     $scope.test = function(){
        console.log("test ka");
     

       
     }
     $scope.return_just_name = function(full_name){
        var set = full_name.split('/');
        return set[set.length-1]
     }
    $scope.set_disabled_search = function(){
        console.log("disabled_search");
        console.log($scope.disabled_search);
        $scope.disabled_search = true;
    }

    $scope.watch_file = function() { 
    
                window.open($rootScope.manage_minutes_fix_this_minute.file_name, '_blank', "width=800, left=230,top=0,height=700");  
      
       
      
    }
     $scope.still_not_complete = function(){
        if(!$scope.my_pictures.flow){
            return true;
        }

        if(!$rootScope.manage_minutes_fix_this_minute){
            return true;
        }
        if(!$rootScope.manage_minutes_fix_this_minute.topic_name || !$rootScope.manage_minutes_fix_this_minute.date || $rootScope.manage_minutes_fix_this_minute.attendee.length ==0 ){
            return true;
        }
        else{
                if($scope.my_pictures.flow.files.length ==0 && $rootScope.manage_minutes_fix_this_minute.pictures.length ==0){
                    return true;
                }

            if($scope.disabled_search == true){
                if($scope.my_file.length ==0){
                    return true;
                }
                
            }
              return false;
        }


return false;       
     }
    
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }
    $scope.delete_picture = function(index_pic){
        $rootScope.manage_minutes_fix_this_minute.pictures.splice(index_pic,1);
    }
$scope.show_my_pictures=function(){

     $scope.show_gallery = true;
     var index;
     console.log("$scope.my_pictures.flow.files")
     console.log($scope.my_pictures.flow.files);;
     console.log($scope.my_pictures.flow.files.length)
     $scope.to_del = [];

     for(index=0;index<$scope.my_pictures.flow.files.length;index++){
        console.log("loop");
        console.log($scope.my_pictures.flow.files[index].name);
        console.log($scope.my_pictures.flow.files[index].size);
        if ($scope.my_pictures.flow.files[index].size > 2000000){
            console.log("remove file");
            console.log($scope.my_pictures.flow.files[index].name);
             $scope.to_del.push($scope.my_pictures.flow.files[index]);

        }
    
     }
    

    for(index=0;index<$scope.to_del.length;index++){
       $scope.my_pictures.flow.files.splice( $scope.my_pictures.flow.files.indexOf($scope.to_del[index]),1);
    }
  

}
    $scope.save_to_server = function(my_modal) {
   $scope.please_wait = true;
      var formData = new FormData();


  
        var index = 0;
        for (index = 0 ;index< $scope.my_pictures.flow.files.length;index++){
        $scope.my_obj = {};
        $scope.my_obj.minutes_id = 0;
        $scope.my_obj.file_name = $scope.my_pictures.flow.files[index].file.name;
            $rootScope.manage_minutes_fix_this_minute.pictures.push($scope.my_obj);
            formData.append("picture"+(index+1), $scope.my_pictures.flow.files[index].file );
        }


       $rootScope.manage_minutes_fix_this_minute.attendee =  $rootScope.manage_minutes_fix_minute_select_attendee;
  
       console.log($rootScope.manage_minutes_fix_this_minute);
      

   if($scope.disabled_search == true){
      $rootScope.manage_minutes_fix_this_minute.file_name = $scope.my_file[0].name;
         formData.append("file", $scope.my_file[0] );
    }
    else{
          $rootScope.manage_minutes_fix_this_minute.file_name = "";
    }
       formData.append("model", angular.toJson( $rootScope.manage_minutes_fix_this_minute));

        $http({
            method: 'PUT',
            url: "/api/minutes/edit",

            headers: { 'Content-Type': undefined },


            data:formData,
            transformRequest: angular.indentity 

        }).
        success(function (data, status, headers, config) {
        $rootScope.manage_minutes_still_same();
                $rootScope.manage_minutes_my_world_wide_minutes =data;
                $scope.close_modal(my_modal);
                $alert({title:'ดำเนินการสำเร็จ', content:'บันทึกข้อมูลเรียบร้อย',alertType:'success',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopSuccess'});

              
           
        }).
        error(function (data, status, headers, config) {
              $scope.please_wait = false;
            $alert({title:'เกิดข้อผิดพลาด', content:'บันทึกข้อมูลไม่สำเร็จ',alertType:'danger',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
        });
    }

 });
app.controller('manage_minutes_show_images_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,Lightbox) {
 $scope.openLightboxModal = function (index) {
    Lightbox.openModal($rootScope.manage_minutes_show_images_of_this_minute.pictures, index);
  };
});
app.controller('manage_minutes_controller', function($scope, $http,$alert,$loading,$timeout,ngDialog,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service) {

$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
                    $scope.nothing_change = true;
                      $rootScope.manage_minutes_my_world_wide_minutes = [];
                       $scope.all_curri_that_have_privileges = [];
                             $scope.corresponding_aca_years = [];
  $scope.$parent.scan_only_privilege_curri('8',$scope.all_curri_that_have_privileges);
}
       $scope.$on("modal.hide", function (event, args) {
     $scope.init();
      
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });


     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
                    $scope.result = {};
  $scope.nothing_change = true;

 $rootScope.manage_minutes_my_world_wide_minutes = [];


$rootScope.manage_minutes_still_same = function(){
    $scope.nothing_change = true;
}
 $scope.show_picture_minute = function(this_minute){
    $rootScope.manage_minutes_show_images_of_this_minute = this_minute;
 }
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
       
         $scope.nothing_change = true;
        $scope.indicator_choosen= {};
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen,8,2).then(function(data) {
 $scope.year_choosen = {};
            $scope.corresponding_aca_years = data;


          });


    }

    $scope.go_to_fix_minute = function(this_minute){

               $http.post(
             '/api/teacher/getname',
             JSON.stringify($scope.curri_choosen.curri_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.manage_minutes_all_teachers_in_curri = data;
              
        $rootScope.manage_minutes_curri_id = $scope.curri_choosen.curri_id;
        $rootScope.manage_minutes_aca_year = $scope.year_choosen.aca_year;
        $rootScope.manage_minutes_fix_this_minute = angular.copy(this_minute);
                var index;
        var inside_index;
        $rootScope.manage_minutes_fix_minute_select_attendee = [];

        for(index =0;index<$rootScope.manage_minutes_fix_this_minute.attendee.length;index++){
            for(inside_index=0;inside_index<$rootScope.manage_minutes_all_teachers_in_curri.length;inside_index++){
                if($rootScope.manage_minutes_all_teachers_in_curri[inside_index].teacher_id  == $rootScope.manage_minutes_fix_this_minute.attendee[index].teacher_id){
                          $rootScope.manage_minutes_fix_minute_select_attendee.push($rootScope.manage_minutes_all_teachers_in_curri[inside_index]);
                }
          
            }
        }
});


       
    }
    $scope.go_to_see_attendee = function(this_minute){
        $rootScope.manage_minutes_see_this_attendee = this_minute.attendee;
    }
    $scope.go_to_create_minute =function(){
           $http.post(
             '/api/teacher/getname',
             JSON.stringify($scope.curri_choosen.curri_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.manage_minutes_all_teachers_in_curri = data;
              
        $rootScope.manage_minutes_curri_id = $scope.curri_choosen.curri_id;
        $rootScope.manage_minutes_aca_year = $scope.year_choosen.aca_year;

});

    }
 

    $scope.remove_minute = function(index_to_remove){
        $scope.nothing_change = false;
        $rootScope.manage_minutes_my_world_wide_minutes.splice(index_to_remove, 1);   
    }



    $scope.find_information = function(){

          console.log("find_information");
        console.log($scope.year_choosen);

        $http.post(
             '/api/minutes/getminutes',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            console.log("fint information");
            console.log($rootScope.manage_minutes_my_world_wide_minutes);
             $scope.nothing_change = true;
               $rootScope.manage_minutes_my_world_wide_minutes = data;
         
             $scope.choose_not_complete = false;
             
            
    
         });

    }
    $scope.close_modal = function(my_modal){
        $scope.init();
        my_modal.$hide();
    }

    $scope.download_file = function(path) { 
        window.open(path, '_blank', "");  
    }

    $scope.save_to_server = function(my_modal){


        if($rootScope.manage_minutes_my_world_wide_minutes.length == 0){
       
            $scope.to_sent  = {};
            $scope.to_sent.curri_id  = $scope.curri_choosen.curri_id;
            $scope.to_sent.aca_year = $scope.year_choosen.aca_year;
   
            $rootScope.manage_minutes_my_world_wide_minutes.push($scope.to_sent);

        }

        console.log("save_to_server");
        console.log($rootScope.manage_minutes_my_world_wide_minutes);
        $http.put(
             '/api/minutes/delete',
             JSON.stringify($rootScope.manage_minutes_my_world_wide_minutes),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.close_modal(my_modal);
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