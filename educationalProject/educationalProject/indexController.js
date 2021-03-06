﻿'use strict';
app.service('alertCaller',function($alert){
    var alertCallerObj = new Object();
    alertCallerObj.success = function(title,success_msg){
        var success_title = title || 'ดำเนินการสำเร็จ';
        var success_content = success_msg || 'บันทึกข้อมูลเรียบร้อย';
        $alert({title: success_title, content: success_content, alertType: 'success',
            placement: 'bottom-right', effect: 'bounce-in', speed: 'slow', typeClass: 'alertPopSuccess'});
    }
    alertCallerObj.error = function(title,error_msg,cause){
        var error_title = title || 'เกิดข้อผิดพลาด';
        var error_content = error_msg || 'บันทึกข้อมูลไม่สำเร็จ ';
        var error_cause = cause || '';
        $alert({title:error_title, content:error_content + error_cause,alertType:'danger',
            placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPop'});
    }
    return alertCallerObj;
});

app.service('fileChecker',function($alert){
    var fileCheckerObj = new Object();
    fileCheckerObj.stdFileChk = function(file){
        var extension = file.name.split('.');
        if (file.size > 25000000) {
            $alert({
                title: 'เกิดข้อผิดพลาด', content: 'ไฟล์ที่เลือกมีขนาดมากกว่า 25 MB', alertType: 'warning',
                placement: 'bottom-right', effect: 'bounce-in', speed: 'slow', typeClass: 'alertPopFileSize'
            });
            return false;
        }
        else if (extension[extension.length - 1] == 'exe' || extension[extension.length - 1] == 'EXE' || extension[extension.length - 1] == 'vb' || extension[extension.length - 1] == 'VB'
            || extension[extension.length - 1] == 'bat' || extension[extension.length - 1] == 'BAT' || extension[extension.length - 1] == 'ini' || extension[extension.length - 1] == 'INI') {
            $alert({
                title: 'เกิดข้อผิดพลาด', content: 'ไม่อนุญาตให้อัพโหลดไฟล์นามสกุลดังกล่าว', alertType: 'warning',
                placement: 'bottom-right', effect: 'bounce-in', speed: 'slow', typeClass: 'alertPopFileSize'
            });
            return false;
        }
        else
        return true;
    }

    fileCheckerObj.imgFileChk = function(file){ /*file parameter as FlowFile object*/
        if(!!{png:1,jpg:1,jpeg:1}[file.getExtension()] != 1)
        return false;
        if (file.size > 2000000){
             return false;
        }
        return true;
     }

    fileCheckerObj.resetFileInput = function(parentOfTarget){
        angular.forEach(
            angular.element(parentOfTarget + " input[type='file']"),
            function (inputElem) {
                angular.element(inputElem).val(null);
            });
    }
    return fileCheckerObj;
});

app.controller('choice_index_controller', function($scope,$anchorScroll, $location,$http,$alert,$cookies,$loading,request_all_curriculums_service_server,$rootScope,$modal,alertCaller) {

    $scope.not_select_curri_and_year = true;
    $scope.year_choosen = {};
     $scope.curri_choosen={};
     $scope.indicator_choosen = {};
     $scope.sub_indicator_choosen = {};
     $scope.select_overall = true;
     $scope.select_year_support_text = 0;
     $scope.select_all_complete = false;
     $scope.already_select_curri = false;
     $scope.show_preview_support_text = 0;
     $scope.current_section_save = [];
     $scope.not_choose_year = true;
       $rootScope.all_curriculums = [];
$scope.send_please_wait = false;
  
   $scope.get_reason_other_evaluate = function(indicator_now){
        $scope.to_sent = {}
        $scope.to_sent.indicator_num = indicator_now;
        $scope.to_sent.curri_id = $scope.curri_choosen.curri_id ;
          $scope.to_sent.aca_year = $scope.year_choosen.aca_year ;
        $http.post(
             '/api/othersevaluation',
             JSON.stringify($scope.to_sent),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $rootScope.reason_other_evaluate = data;
             $scope.choose_not_complete = false;
         });
    }

$scope.selectThisSub_not_link = function(sub_indicator){
        $scope.sendSectionSaveAndGetSupportText(sub_indicator);
    var my_pattern = [];
    var index;
    for(index=1;index< sub_indicator.sub_indicator_num ;index++){
    my_pattern.push(0);
    }
    my_pattern.push(1);

    for(index=sub_indicator.sub_indicator_num;index< $rootScope.length_of_sub_indicators_now;index++){
    my_pattern.push(0)
    }
       $scope.which_active_sub = my_pattern;
}

$rootScope.selectThisSub = function(sub_indicator){
        $scope.sendSectionSaveAndGetSupportText_to_link(sub_indicator);
    var my_pattern = [];
    var index;


    for(index=1;index< sub_indicator ;index++){
    my_pattern.push(0);
    }
    my_pattern.push(1);

    for(index=sub_indicator;index< $rootScope.length_of_sub_indicators_now;index++){
    my_pattern.push(0)
    }
    $rootScope.active_sub_like_this(my_pattern);

}
$rootScope.selectThisSub_and_move = function(sub_indicator){
        $scope.sendSectionSaveAndGetSupportText_to_link(sub_indicator);
    var my_pattern = [];
    var index;

    for(index=1;index< sub_indicator ;index++){
    my_pattern.push(0);
    }
    my_pattern.push(1);

    for(index=sub_indicator;index< $rootScope.length_of_sub_indicators_now;index++){
    my_pattern.push(0)
    }

    $rootScope.active_sub_like_this(my_pattern);
          $location.hash('link_sub_reason');

$anchorScroll();
$location.hash(null)
}
$rootScope.go_to_edit_reason = function (my_modal,sub_indicator){
        $scope.sendSectionSaveAndGetSupportText_to_link(sub_indicator);
my_modal.$hide();
var my_pattern = [];
var index;

for(index=1;index< sub_indicator ;index++){
my_pattern.push(0);
}
my_pattern.push(1);

for(index=sub_indicator;index< $rootScope.length_of_sub_indicators_now;index++){
my_pattern.push(0)
}

$rootScope.active_sub_like_this(my_pattern);

$location.hash('edit_reason_now');

$anchorScroll();
$location.hash(null)
}

  $scope.get_reason_self_evaluate = function(indicator_now,sub_indicator_now){
        $scope.section_save_to_send = new Object();
             $scope.section_save_to_send.teacher_id = "";
             $scope.section_save_to_send.detail  = "";
             $scope.section_save_to_send.date  = "";
             $scope.section_save_to_send.time  = "";

             $scope.section_save_to_send.aca_year = $scope.year_choosen.aca_year;
             $scope.section_save_to_send.indicator_num  = indicator_now;
             $scope.section_save_to_send.sub_indicator_num   = sub_indicator_now;
             $scope.section_save_to_send.curri_id = $scope.curri_choosen.curri_id;
                $http.post(
                     '/api/sectionsave/getsectionsave',
                     JSON.stringify($scope.section_save_to_send),
                     {
                         headers: {
                             'Content-Type': 'application/json'
                         }
                     }
                 ).success(function (data) {
                    $rootScope.display_self_support= data;
                    $rootScope.sub_indicator_now_for_link = sub_indicator_now;
                 });
    }
        $rootScope.download_file_now = function(path) { 
        window.open(path, '_blank', "");  
    }
    $rootScope.clear_choosen = function(){
       $scope.not_select_curri_and_year = true;
        $scope.year_choosen = {};
         $scope.curri_choosen={};
         $scope.indicator_choosen = {};
         $scope.sub_indicator_choosen = {};
         $scope.select_overall = true;
         $scope.select_year_support_text = 0;
         $scope.select_all_complete = false;
         $scope.already_select_curri = false;
         $scope.show_preview_support_text = 0;
         $scope.current_section_save = [];
         $scope.not_choose_year = true;
         $scope.corresponding_aca_years = [];
         $scope.send_please_wait = false;
    }

     $rootScope.update_curri_choosen = function(){
         if(!angular.equals($scope.curri_choosen,{}) ){
            $scope.curri_choosen = $rootScope.all_curriculums.find(t => t.curri_id == $scope.curri_choosen.curri_id);
         }
     }
        $scope.can_watch_result = function(){
    if($scope.$parent.already_login == true){
        if($rootScope.current_user.user_type == 'ผู้ดูแลระบบ'){
            return true;
        }
        if(!$rootScope.current_user.privilege[$scope.curri_choosen.curri_id]){
            return false;
        }
        if($rootScope.have_privilege(30,2,$scope.curri_choosen.curri_id,$scope.year_choosen.aca_year)){
            return true;
        }
    }
       return false;
    }

$scope.download_plain_book = function(){

    $scope.to_sent_to_get_plain = {};
    $scope.to_sent_to_get_plain.curri_id = $scope.curri_choosen.curri_id;
    $scope.to_sent_to_get_plain.aca_year = $scope.year_choosen.aca_year;

   $http.post(
             '/api/sectionsave/genaunsar',
             JSON.stringify($scope.to_sent_to_get_plain),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             },
             {responseType: 'arraybuffer'}
         ).success(function (data) {
           var file = new Blob([data], {type: 'application/msword'});
           if(window.navigator.msSaveOrOpenBlob) 
                {
                    window.navigator.msSaveOrOpenBlob(file, 'generateSAR.doc');
                }
           else{
           var fileURL = URL.createObjectURL(file);

           window.open(fileURL);
           }
         });
}
        $scope.can_download_plain = function(){

    if($scope.$parent.already_login == true){
        if($rootScope.current_user.user_type == 'ผู้ดูแลระบบ'){
            return true;
        }
        if(!$rootScope.current_user.privilege[$scope.curri_choosen.curri_id]){
            return false;
        }
        if($rootScope.have_privilege(32,2,$scope.curri_choosen.curri_id,$scope.year_choosen.aca_year)){
            return true;
        }
    }
       return false;
    }

    $rootScope.can_edit_reason = function(){  
    if($scope.$parent.already_login == true){
        if(!$rootScope.current_user.privilege[$scope.curri_choosen.curri_id]){
            return false;
        }
        if($rootScope.have_privilege(15,3,$scope.curri_choosen.curri_id,$scope.year_choosen.aca_year)){
            return true;
        }
    }
       return false;
    }

    $rootScope.can_watch_reason = function(){
    if($scope.$parent.already_login == true){
        if($rootScope.current_user.user_type == 'ผู้ดูแลระบบ'){
            return true;
        }
        if(!$rootScope.current_user.privilege[$scope.curri_choosen.curri_id]){
            return false;
        }
        if($rootScope.have_privilege(15,2,$scope.curri_choosen.curri_id,$scope.year_choosen.aca_year)){
            return true;
        }
    }
       return false;
    }

     $scope.init_var = function(){
    $scope.not_select_curri_and_year = true;
    $scope.year_choosen = {};
     $scope.curri_choosen={};
     $scope.indicator_choosen = {};
     $scope.sub_indicator_choosen = {};
     $scope.select_overall = true;
     $scope.select_year_support_text = 0;
     $scope.select_all_complete = false;
     $scope.already_select_curri = false;
     $scope.show_preview_support_text = 0;
     $scope.current_section_save = [];
     $scope.not_choose_year = true;
       $rootScope.all_curriculums = [];
$scope.send_please_wait = false;
$rootScope.current_curriculum = "";
$rootScope.current_aca_year = "";
$rootScope.current_indicator = "";
     }
        request_all_curriculums_service_server.async().then(function(data) {
            $rootScope.all_curriculums = data;
          });

        // $http.get('/api/curriculum').success(function (data) {
            
        //     $rootScope.all_curriculums = data;
        //   });
            
    $scope.clear_select_year_support_text_choosen = function(){
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

    var my_pattern = [];
    var index;
    my_pattern.push(1);
    for(index=1;index< $scope.length_of_indicators_now;index++){
    my_pattern.push(0)
    }
    $scope.which_active = my_pattern;
    }
    $scope.sendCurriAndGetYears = function (curri) {
        $scope.select_all_complete = false;
         $scope.not_select_curri_and_year = true;
         $scope.already_select_curri = true;
           $scope.not_choose_year = true;
           $scope.year_choosen = {};
            $scope.$parent.not_choose_curri_and_year_yet = true;
 
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
    $scope.check_curri = function(){
        if ($scope.already_select_curri == false){
            alertCaller.error(null,'กรุณาเลือกหลักสูตรที่ต้องการก่อน');     
        }
    }

    $rootScope.active_sub_like_this = function(pattern){
$scope.which_active_sub = pattern;
    }
     $scope.sendYearAndGetIndicators = function (year) {
         $scope.select_all_complete = true;
        if ($scope.select_all_complete  == true){
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
                   $scope.not_choose_year = false;
                   $scope.which_active = [];
                   $scope.which_active.push(1);
                   var index;

                   $scope.length_of_indicators_now = $scope.corresponding_indicators.length;
                   for(index = 0;index<$scope.corresponding_indicators.length;index++){
                         $scope.which_active.push(0);
                   }
                 
                   $scope.$parent.not_choose_curri_and_year_yet = false;
             });
         }
         else if ($scope.already_select_curri == true){
             alertCaller.error(null,'กรุณาเลือกปีการศึกษา');
         }
         else{
             alertCaller.error(null,'กรุณาเลือกหลักสูตรและปีการศึกษา');
         }
    }

    $scope.send_sub_indicator = function(sub_indicator){
        $scope.sub_indicator_choosen = sub_indicator;
        $scope.sendSectionSaveAndGetSupportText();
    }
$scope.go_to_indicator = function(indicator){
    $scope.sendIndicatorAndGetSubIndicators(indicator);
    $scope.select_this_indi(indicator.indicator_num);
}

$scope.go_to_indicator_by_number = function(indicator_num){
var indicator;
var index;

        for(index=0;index<$scope.corresponding_indicators.length;index++){
            if($scope.corresponding_indicators[index].indicator_num == indicator_num){ 
                indicator = $scope.corresponding_indicators[index];
            }
        }
     
    $scope.sendIndicatorAndGetSubIndicators(indicator);
    $scope.select_this_indi(indicator_num);
    $location.hash(null)
    $anchorScroll();
$location.hash(null)
}

     $scope.sendIndicatorAndGetSubIndicators_to_pass = function (indicator,sub_indicator) {
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
              $scope.which_active_sub = [];
              $rootScope.length_of_sub_indicators_now =$scope.corresponding_sub_indicators.length;

             if($scope.can_watch_reason() == true){
                $scope.sendSectionSaveAndGetSupportText_to_link(sub_indicator.sub_indicator_num);
             }
               $scope.select_this_indi(indicator.indicator_num);

    $scope.selectThisSub_not_link(sub_indicator);
    $location.hash('link_sub_reason');

$anchorScroll();
$location.hash(null)
         });

    }

$scope.go_to_indicator_and_sub_by_number = function(indicator_num,sub_indicator){
var indicator;
var index;

        for(index=0;index<$scope.corresponding_indicators.length;index++){
            if($scope.corresponding_indicators[index].indicator_num == indicator_num){
                indicator = $scope.corresponding_indicators[index];
            }
        }
    $scope.sendIndicatorAndGetSubIndicators_to_pass(indicator,sub_indicator); 
}

$scope.select_this_indi = function(indicator){
   
    var my_pattern = [];
    var index;

    my_pattern.push(0);
    for(index=1;index< indicator ;index++){
    my_pattern.push(0);
    }
    my_pattern.push(1);

    for(index=indicator;index< $scope.length_of_indicators_now;index++){
    my_pattern.push(0)
    }
    $scope.which_active = my_pattern;
}

     $scope.sendIndicatorAndGetSubIndicators = function (indicator) {
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
              $scope.which_active_sub = [];
              $rootScope.length_of_sub_indicators_now =$scope.corresponding_sub_indicators.length;
                $scope.which_active_sub.push(1);
              var index;
                   for(index = 1;index<$scope.corresponding_sub_indicators.length;index++){
                         $scope.which_active_sub.push(0);
                   }
             if($scope.can_watch_reason() == true){
                $scope.sendSectionSaveAndGetSupportText_to_link(1);
             }
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
        $scope.self_evaluated = true;
        $scope.others_evaluated = true;

        $scope.self_sum_overall = 0;
        $scope.self_count_overall = 0;
            $scope.others_count_overall = 0;
         $scope.others_sum_overall = 0;
            var index;
            for(index=0;index<$scope.evaluation_overall.length;index++){

                if(!$scope.evaluation_overall[index].self_time || !$scope.evaluation_overall[index].other_time){
                    $scope.evaluation_overall[index].complete_both = false;
                }
                else{
                    $scope.evaluation_overall[index].complete_both = true;
                }

                 if(!$scope.evaluation_overall[index].self_time){
                   $scope.self_evaluated = false;
                }else{
                     $scope.self_count_overall =  $scope.self_count_overall +1;
                    $scope.self_sum_overall = $scope.self_sum_overall +$scope.evaluation_overall[index].sub_indicator_result[0].sub_indicator_self_result;
                }
                if(!$scope.evaluation_overall[index].other_time){
                       $scope.others_evaluated = false;
                }else{
                      $scope.others_sum_overall = $scope.others_sum_overall +$scope.evaluation_overall[index].sub_indicator_result[0].sub_indicator_other_result;
                    $scope.others_count_overall = $scope.others_count_overall +1;
                }
            }

            $scope.others_final_overall = $scope.others_sum_overall / $scope.others_count_overall;
           $scope.self_final_overall   = $scope.self_sum_overall/$scope.self_count_overall ;
         });
     }

    $scope.sendIndicatorCurriAndGetEvaluation = function () {
        $scope.this_indicator_show = {};
        var index;
        for(index=0;index<$scope.evaluation_overall.length;index++){
            if($scope.evaluation_overall[index].indicator_num == $scope.indicator_choosen.indicator_num){
                $scope.this_indicator_show = $scope.evaluation_overall[index];
            }
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
     
            $scope.corresponding_evidences = data;
            if($scope.corresponding_evidences.length==0){            
                 $scope.have_evidences = false;
            }
        else{     
             $scope.have_evidences = true;
        }
         });
    }

$scope.watch_support_text_from_other_year= function(){

if($scope.select_year_support_text != 0){
    if(!!$scope.show_preview_support_text){
$modal({
    size:'lg',
    keyboard:false,
    content:$scope.show_preview_support_text,
    contentTemplate:'/template/previewsectionsave.html',
    autoDestroy:true
});
    }
    else{
     $modal({
    size:'lg',
    keyboard:false,
    content:'ไม่พบข้อมูลสนับสนุนการประเมินตนเองในปีการศึกษาดังกล่าว',
    contentTemplate:'/template/previewsectionsave.html',
    autoDestroy:true
});   
    }
  }
  else{
      alertCaller.error(null,'กรุณาเลือกปีการศึกษา');            
  }
}
$scope.get_support_content_from_other_year = function (my_modal) {
    CKEDITOR.instances['support_text'].setData($scope.show_preview_support_text);

 my_modal.$hide();
     alertCaller.success(null,'ดึงข้อมูลเรียบร้อย');
}

    $scope.find_curri_information = function(){
        $scope.to_get_url = "/api/curriculum/getcurridetail/"+$scope.curri_choosen.curri_id;
 $http.get($scope.to_get_url).success(function (data) {
                   $rootScope.result_curri_info = data;
                         
                if($rootScope.result_curri_info.level == 1){
                    $rootScope.result_curri_info.level = "ปริญญาตรี"
                }
                else if ($rootScope.result_curri_info.level == 2){
                    $rootScope.result_curri_info.level = "ปริญญาโท"
                }
                else{
                   $rootScope.result_curri_info.level = "ปริญญาเอก"
                }
           });
    }

$scope.prepare_to_watch_support_text = function(){
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
            if(!data){
                alertCaller.error(null,'เล่มรายงานยังไม่ถูกอัพโหลด');
            }
            else{
                $scope.download_file(data);
            }
         })
         .error(function (data) {
             alertCaller.error(null,'ไม่สามารถดาวน์โหลดได้ ',data.message);
         });
    }
    else{
        alertCaller.error(null,'กรุณาเลือกหลักสูตรและปีการศึกษาก่อน');
    }
}
$scope.send_support_text_change_to_server = function(){
    $scope.send_please_wait = true;
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
             alertCaller.success();
                    $scope.send_please_wait = false;
         })
    .error(function(data, status, headers, config) {
                $scope.send_please_wait = false;
                alertCaller.error(null,null,data.message);
  });
}
 $scope.sendSectionSaveAndGetPreviewSupportText = function () {
    if($scope.select_year_support_text!=0){
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
                     '/api/sectionsave/getsectionsave',
                     JSON.stringify($scope.section_save_to_send),
                     {
                         headers: {
                             'Content-Type': 'application/json'
                         }
                     }
                 ).success(function (data) {
                    $scope.show_preview_support_text= data.detail;
                    window.currentcurrisel = $scope.curri_choosen.curri_id;
           $http.post(
             '/api/evidence/getallevidence',
             JSON.stringify($scope.curri_choosen.curri_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
       window.big_chunk = data;
         });
                 });
                 }
                 else{
                     alertCaller.error(null,'กรุณาเลือกปีการศึกษาก่อน');
                 }
    }

        $scope.sendSectionSaveAndGetSupportText_to_link = function (number_of_sub) {
var index;
for(index=0;index<$scope.corresponding_sub_indicators.length;index++){
    if( $scope.corresponding_sub_indicators[index].sub_indicator_num == number_of_sub ){
            $scope.sub_indicator_choosen = $scope.corresponding_sub_indicators[index];
    }
}

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
             '/api/sectionsave/getsectionsave',
             JSON.stringify($scope.section_save_to_send),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.current_section_save = data;
            CKEDITOR.instances['support_text'].setData(data.detail);
            window.currentcurrisel = $scope.curri_choosen.curri_id;
           $http.post(
             '/api/evidence/getallevidence',
             JSON.stringify($scope.curri_choosen.curri_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
       window.big_chunk = data;
         });
         });
    }

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
             '/api/sectionsave/getsectionsave',
             JSON.stringify($scope.section_save_to_send),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.current_section_save = data;
            CKEDITOR.instances['support_text'].setData(data.detail);
         });
    }

    $scope.getresultcolor = function(sub_ind_result,ind_result){
        return {resultGreen: sub_ind_result.sub_indicator_other_result>sub_ind_result.sub_indicator_self_result  &&  ind_result.complete_both , resultRed:sub_ind_result.sub_indicator_self_result>sub_ind_result.sub_indicator_other_result  &&  ind_result.complete_both };
    }
});

app.controller('add_aca_year', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,alertCaller,$filter) {
    var thisctrl = this;
    $scope.init = function(){
        $scope.curri_choosen = {};
               $scope.new_curri_academic = {};
        $scope.new_curri_academic.aca_year = "";
 $scope.please_wait = false;
      $scope.all_curri_that_have_privileges = [];
      if($rootScope.current_user.user_type == 'ผู้ดูแลระบบ'){
        $scope.all_curri_that_have_privileges = $rootScope.all_curriculums;
      }else{
         $scope.$parent.scan_only_privilege_curri('2',$scope.all_curri_that_have_privileges);
      }
    }
$scope.init();
       $scope.$on("modal.hide", function (event, args) {
           thisctrl.add_year_form.$setPristine();
           $scope.init();
    });
  $scope.$on("modal.show", function (event, args) {
      thisctrl.add_year_form.$setPristine();
      $scope.init();
    var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
        if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
    }
    });
  $scope.still_not_complete = function(){
    if(!$scope.new_curri_academic){
        return true;
    }
    if(angular.equals({},$scope.curri_choosen) || !$scope.new_curri_academic.aca_year){
        return true;
    }
    if(angular.isNumber($scope.new_curri_academic.aca_year)==false || $scope.new_curri_academic.aca_year <= 0){
           return true;
    }
    return false;
  }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
    $scope.add_aca_year_to_server = function(my_modal){
         $scope.please_wait = true;
if( $scope.curri_choosen!= "none" && $scope.new_curri_academic.aca_year != ""){
        $scope.new_curri_academic.curri_id = $scope.curri_choosen.curri_id;
              $http.post(
             '/api/curriculumacademic/add',
             JSON.stringify($scope.new_curri_academic),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             alertCaller.success(null,'เพิ่มปีการศึกษาเรียบร้อย');
                  my_modal.$hide();
         })
         .error(function(data, status, headers, config) {
 $scope.please_wait = false;
                  if(status==400){
                      alertCaller.error(null,data.message);
     }
  });
     }else{
         $scope.please_wait = false;
         alertCaller.error(null,'กรุณาเลือกหลักสูตรและระบุปีการศึกษา');
     }
    }
});

app.controller('manage_curriculum_controller', function($scope, $http,$alert,request_all_curriculums_service_server,$rootScope,alertCaller){
$scope.$on('modal.show',function(event,args){
    $rootScope.curriculum_list = [];
    request_all_curriculums_service_server.get_all_curri().then(function(data){
        if($rootScope.is_admin() == true){
            $rootScope.curriculum_list = data;
        }
        else{
            var index;
            for(index = 0;index < data.length;index++){
                if(!!$rootScope.current_user.president_in[data[index].curri_id]){
                    $rootScope.curriculum_list.push(data[index]);
                }
            }
        }
    });
});
$scope.go_to_create = function(){
    $rootScope.curri_ctrl_mode = 1;
};
$scope.go_to_edit = function(index_to_edit){
    $rootScope.curri_ctrl_mode = 2;
    $rootScope.pass_curri_to_edit = angular.copy($rootScope.curriculum_list[index_to_edit]);
};
});

app.controller('create_edit_curriculum_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,alertCaller) {
    var thisctrl = this;
  $scope.$on("modal.show", function (event, args) {
      $scope.please_wait = false;
      $scope.curri_obj = {};
      $scope.curri_obj.level = {};
      if ($rootScope.curri_ctrl_mode === 2) {
          $scope.mode_txt = 'แก้ไขข้อมูล';
          $scope.curri_obj = $rootScope.pass_curri_to_edit;
      }
      else{
          $scope.mode_txt = 'สร้าง';
          $scope.curri_obj.curri_id = '0';
          $scope.curri_obj.year= "";
      }
      thisctrl.create_edit_curri_form.$setPristine();
    });

    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }

$scope.still_not_complete = function(){
    if(!$scope.curri_obj){
        return true;
    }
 if (!$scope.curri_obj.curr_tname || !$scope.curri_obj.curr_ename || !$scope.curri_obj.degree_t_full ||  !$scope.curri_obj.degree_t_bf || !$scope.curri_obj.degree_e_full||  !$scope.curri_obj.degree_e_bf ||  !$scope.curri_obj.level || !$scope.curri_obj.period ){
    return true;
 }
 if($scope.curri_obj.level != 1 && $scope.curri_obj.level != 2 && $scope.curri_obj.level != 3){
    return true;
 }
 return false;
}

    $scope.save_to_server = function(my_modal){
 $scope.please_wait = true;
         $http.post(
             '/api/curriculum/insertoredit',
             JSON.stringify($scope.curri_obj),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
                 $rootScope.all_curriculums = data;
                 if ($rootScope.is_admin() == true) {
                     $rootScope.curriculum_list = angular.copy(data);
                 }
                 else {
                     $scope.templist = [];
                     var index;
                     for (index = 0; index < data.length; index++) {
                         if (!!$rootScope.current_user.president_in[data[index].curri_id]) {
                             $scope.templist.push(data[index]);
                         }
                     }
                     $rootScope.curriculum_list = $scope.templist;
                 }
                 if($rootScope.curri_ctrl_mode === 1){
                     alertCaller.success(null,'สร้างหลักสูตรเรียบร้อย');
                     $rootScope.clear_choosen();
                 }
                 else{
                    alertCaller.success(null,'แก้ไขข้อมูลหลักสูตรเรียบร้อย');
                    $rootScope.update_curri_choosen();
                 }
                 my_modal.$hide();
         })
      .error(function(data, status, headers, config) {
  $scope.please_wait = false;
  alertCaller.error(null,null,data.message);
  });
}
});



app.service('curriculum_related_data',function($rootScope){
    var serviceObj = new Object();
    serviceObj.getgradyear = function(wantedcurri){
        var targetcurri = $rootScope.all_curriculums.find(t => t.curri_id == wantedcurri.curri_id);
        if(targetcurri.period != 0)
            return targetcurri.period;
        else {
            if(targetcurri.level == 1)
                return 4;
            else if(targetcurri.level == 2 || targetcurri.level == 3)
                return 2;
            else
                return 0;
        }
    };
    serviceObj.getlevel = function(wantedcurri){
        return $rootScope.all_curriculums.find(t => t.curri_id == wantedcurri.curri_id).level;
    }
    return serviceObj;
});





app.controller('stat_graduated_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller,$filter,$q,curriculum_related_data) {
$scope.init =function() {
     $scope.choose_not_complete = true;
     $scope.year_choosen = {};
     $scope.curri_choosen = {};
     $scope.result = {};
     $scope.gradtime = 0;
     $scope.all_curri_that_have_privileges = [];
     $scope.corresponding_aca_years = [];
   $scope.$parent.scan_only_privilege_curri('11',$scope.all_curri_that_have_privileges);
}
  
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {};
      
        var promise = request_years_from_curri_choosen_service.async($scope.curri_choosen,11,2).then(function(data) {
        $scope.corresponding_aca_years = data;
        return $q.resolve("");
        });
        return promise;
    }
     $scope.$on("modal.hide", function (event, args) {
     $scope.init();
    });
  $scope.$on("modal.show", function (event, args) {
        $scope.init();
//**********************************/
    var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
    if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];

        //Use promise functional to wait for aca_year api request
        $scope.sendCurriAndGetYears().then(function(){
            var target_aca_year = $filter('filter')($scope.corresponding_aca_years, {aca_year: $rootScope.current_aca_year},true);
            if(target_aca_year.length > 0){
                $scope.year_choosen = target_aca_year[0];
                $scope.find_information();
            }
        });
    }
    });

    $scope.find_information = function(){
        $http.post(
             '/api/studentstatusother',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.gradtime = curriculum_related_data.getgradyear($scope.curri_choosen);
              $scope.result = data;
             $scope.choose_not_complete = false;
         });
    }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
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
               alertCaller.success();         
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  }); 
    }
});

app.controller('stat_student_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller,$filter,$q) {
$scope.init =function() {
     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {};
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
    var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
    if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.sendCurriAndGetYears().then(function(){
            var target_aca_year = $filter('filter')($scope.corresponding_aca_years, {aca_year: $rootScope.current_aca_year},true);
            if(target_aca_year.length > 0){
                $scope.year_choosen = target_aca_year[0];
                $scope.find_information();
            }
        });
    }
    });
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {};

            var promise = request_years_from_curri_choosen_service.async($scope.curri_choosen,13,2).then(function(data) {
            $scope.corresponding_aca_years = data;
            return $q.resolve("");
          });
          return promise;
    }

    $scope.find_information = function(){
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
         });
    }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
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
                 alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  }); 
    }
});

app.controller('stat_new_student_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller,curriculum_related_data,$filter,$q) {
$scope.init =function() {
     $scope.choose_not_complete = true;
       $scope.year_choosen = {};
              $scope.curri_choosen = {};
                   $scope.result = {};
                    $scope.all_curri_that_have_privileges = [];
                          $scope.corresponding_aca_years = [];
  $scope.$parent.scan_only_privilege_curri('12',$scope.all_curri_that_have_privileges);
}
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {};
        var promise = request_years_from_curri_choosen_service.async($scope.curri_choosen,12,2).then(function(data) {
            $scope.corresponding_aca_years = data;
            return $q.resolve("");
        });
        return promise;
    }
     $scope.$on("modal.hide", function (event, args) {
     $scope.init(); 
    });
  $scope.$on("modal.show", function (event, args) {
    $scope.init();
    var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
    if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.sendCurriAndGetYears().then(function(){
            var target_aca_year = $filter('filter')($scope.corresponding_aca_years, {aca_year: $rootScope.current_aca_year},true);
            if(target_aca_year.length > 0){
                $scope.year_choosen = target_aca_year[0];
                $scope.find_information();
            }
        });
    }
    });

    $scope.find_information = function(){
        $scope.year_choosen.lv = curriculum_related_data.getlevel($scope.curri_choosen);
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
         });
    }

    $scope.is_data_invalid = function(){
        for(var a in $scope.result){
            if($scope.result[a] === undefined){
            return true;
            }
        }
        return false;
    }

    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }

    $scope.save_to_server = function(my_modal){
        $scope.result.lv = curriculum_related_data.getlevel($scope.curri_choosen);
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
                  alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
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

app.controller('evaluate_by_me_controller', function($scope, $alert,$http,request_years_from_curri_choosen_service,$rootScope,alertCaller,$filter,$q) {
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
    var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
    if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.sendCurriAndGetYears().then(function(){
            var target_aca_year = $filter('filter')($scope.corresponding_aca_years, {aca_year: $rootScope.current_aca_year},true);
            if(target_aca_year.length > 0){
                $scope.year_choosen = target_aca_year[0];
                $scope.find_indicators().then(function(){
                    if(!!$rootScope.current_indicator){
                        var target_indicator = $filter('filter')($scope.corresponding_indicators, {aca_year: $rootScope.current_indicator.aca_year,indicator_num: $rootScope.current_indicator.indicator_num},true);
                        if(target_indicator.length > 0){
                            $scope.indicator_choosen = target_indicator[0];
                            $scope.get_results();
                        }
                    }
                });
            }
        });
    }
    });

       $scope.close_modal = function(my_modal){
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
         if($scope.corresponding_results[index].evaluation_score == 0){
                 return true;
            }
    }

    if($scope.corresponding_results.length == 0){
        return true;
    }
    return false;
   }

      $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      
        var promise = request_years_from_curri_choosen_service.async($scope.curri_choosen,4,2).then(function(data) {
$scope.corresponding_indicators = [];
            $scope.corresponding_aca_years = data;
            $scope.choose_not_complete = true;
            return $q.resolve("");
          });
          return promise;
    }

    $scope.find_indicators = function(){
$scope.indicator_choosen = {};
        var promise = $http.post(
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
             return $q.resolve("");
         });
         return promise;
    }

    $scope.get_results= function(){
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
             alertCaller.success();
              my_modal.$hide();
              $rootScope.clear_choosen();
         })
         .error(function (data, status, headers, config) {
             alertCaller.error(null,null,data.message);
        });
    }
});

app.controller('evaluate_by_other_controller', function($scope,$rootScope, $alert,$http,request_years_from_curri_choosen_service,fileChecker,alertCaller,$filter,$q) {
    $scope.init =function() {
  $scope.please_wait = false;
     $scope.choose_not_complete = true;
           $scope.year_choosen = {};
              $scope.curri_choosen = {}
  $scope.files = [];
$scope.indicator_choosen = {};
 $scope.corresponding_aca_years = [];
                 $scope.corresponding_indicators = [];
  $scope.disabled_search = false;
  $scope.all_curri_that_have_privileges = [];
       var index;
    var index2;

for(index=0;index<$rootScope.all_curriculums.length;index++){
    for(index2 = 0;index2 < $rootScope.current_user.curri_id_in.length;index2++){     
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
        $scope.disabled_search = true;
    }
    $scope.watch_file = function() {
                window.open($scope.corresponding_results.file_name, '_blank', "width=800, left=230,top=0,height=700");  
    }

    $scope.back_to_default = function () {
        if (!$scope.corresponding_results.file_name) {

        }
        else {
            $scope.disabled_search = false;
        }
        angular.forEach(
            angular.element("input[type='file']"),
            function (inputElem){
                angular.element(inputElem).val(null);
            });
        $scope.files = [];
    }
   $scope.still_not_complete = function(){
    var index;
    if(!$scope.corresponding_results){
        return true;
    }
    /*if($scope.disabled_search==true){
        if($scope.files.length ==0){
            return true;
        }
    }*/

    if($scope.corresponding_results.evaluation_detail.length == 0){
        return true;
    }
    for(index=0;index<$scope.corresponding_results.evaluation_detail.length;index++){
        if (!$scope.corresponding_results.evaluation_detail[index].evaluation_score){
            return true;
        }
         if ($scope.corresponding_results.evaluation_detail[index].evaluation_score == 0){
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
    var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
    if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.sendCurriAndGetYears().then(function(){
            var target_aca_year = $filter('filter')($scope.corresponding_aca_years, {aca_year: $rootScope.current_aca_year},true);
            if(target_aca_year.length > 0){
                $scope.year_choosen = target_aca_year[0];
                $scope.find_indicators().then(function(){
                    if(!!$rootScope.current_indicator){
                        var target_indicator = $filter('filter')($scope.corresponding_indicators, {aca_year: $rootScope.current_indicator.aca_year,indicator_num: $rootScope.current_indicator.indicator_num},true);
                        if(target_indicator.length > 0){
                            $scope.indicator_choosen = target_indicator[0];
                            $scope.get_results();
                        }
                    }
                });
            }
        });
    }

    });

  $scope.$on("fileSelected", function (event, args) {
      $scope.$apply(function () {
          $scope.files = [];
          if (!fileChecker.stdFileChk(args.file)) {
              fileChecker.resetFileInput('');
          }
          else {
              $scope.files.push(args.file);
          }
      });
    });
     $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
        var promise = request_years_from_curri_choosen_service.async($scope.curri_choosen).then(function(data) {
            var ind;
            var now = new Date(); 
            var nowDate = new Date(now.getTime() + (420 + (420-(now.getTimezoneOffset() * -1))) * 60000);
            var expTime = new Date("August 1, 2017 00:00:00 UTC");
            $scope.corresponding_aca_years = [];
            for(ind = 0;ind < data.length;ind++){
                if(data[ind].aca_year != 2559)
                    $scope.corresponding_aca_years.push(data[ind]);
                else{
                    if(nowDate < expTime)
                        $scope.corresponding_aca_years.push(data[ind]);
                }
            }
             $scope.choose_not_complete = true;
        $scope.corresponding_indicators = [];
                    $scope.please_wait = false;
                    return $q.resolve("");
          });
          return promise;
    }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
    $scope.find_indicators = function(){
$scope.indicator_choosen = {};
        var promise = $http.post(
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
            return $q.resolve("");
         });
         return promise;
    }

    $scope.get_results= function(){
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
        $scope.please_wait = false;
             $scope.corresponding_results = data;
             $scope.choose_not_complete = false;
                        if(!$scope.corresponding_results.file_name){
              $scope.disabled_search = true;
        }
         });
    }

 $scope.save_to_server = function(my_modal) {
          $scope.please_wait = true;
      var formData = new FormData();
$scope.to_sent = {};

if( $scope.files.length != 0){
      $scope.to_sent.file_name = $scope.files[0].name;
}
var index;
for(index =0;index<$scope.corresponding_results.evaluation_detail.length;index++){
    $scope.corresponding_results.evaluation_detail[index].assessor_id = $rootScope.current_user.user_id;
}

 formData.append("model", angular.toJson($scope.corresponding_results));
  formData.append("file", $scope.files[0]);
        $http({
            method: 'PUT',
            url: "/api/othersevaluation",
            headers: { 'Content-Type': undefined },
            data:formData,
            transformRequest: angular.indentity
        }).
        success(function (data, status, headers, config) {
                   my_modal.$hide();
                   alertCaller.success();           
        }).
        error(function (data, status, headers, config) {
              $scope.please_wait = false;
              alertCaller.error(null,null,data.message);
        });
}
});

app.controller('see_evaluate_by_other_controller', function($scope,$rootScope, $alert,$http,request_years_from_curri_choosen_service) {
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
  $scope.$parent.scan_only_privilege_curri('28',$scope.all_curri_that_have_privileges);
   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
}
    $scope.download_file = function(path) { 
        window.open(path, '_blank', "");  
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
      
              request_years_from_curri_choosen_service.async($scope.curri_choosen,28,2).then(function(data) {
            $scope.corresponding_aca_years = data;
             $scope.choose_not_complete = true;
        $scope.corresponding_indicators = [];
            $scope.please_wait = false;
          });
    }

    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
 
    $scope.find_indicators = function(){
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
      $scope.choose_not_complete = true;
             $scope.corresponding_indicators = data;
                    $scope.please_wait = false;
         });
    }

    $scope.get_results= function(){
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
        $scope.please_wait = false;
             $scope.corresponding_results = data;
             $scope.choose_not_complete = false;
         });
    }
});

app.controller('upload_aun_controller', function($scope, $alert,$http,request_years_from_curri_choosen_service,$rootScope,fileChecker,alertCaller,$filter,$q) {

    $scope.init =function() {
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
    var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
    if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.sendCurriAndGetYears().then(function(){
            var target_aca_year = $filter('filter')($scope.corresponding_aca_years, {aca_year: $rootScope.current_aca_year},true);
            if(target_aca_year.length > 0){
                $scope.year_choosen = target_aca_year[0];
                $scope.find_information();
            }
        });
    }
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
      
              var promise = request_years_from_curri_choosen_service.async($scope.curri_choosen,14,2).then(function(data) {
            $scope.corresponding_aca_years = data;
            return $q.resolve("");
          });
          return promise;
    }
   $scope.$on("fileSelected", function (event, args) {
       $scope.$apply(function () {
           $scope.files = [];
           if (!fileChecker.stdFileChk(args.file)) {
               fileChecker.resetFileInput('');
           }
           else {
               $scope.files.push(args.file);
           }
       });
    });
        $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }

    $scope.save_to_server = function(my_modal) {
   $scope.please_wait = true;
      $scope.model  = {"file_name":$scope.files[0].name,"personnel_id":$rootScope.current_user.user_id,"date":"","curri_id":$scope.curri_choosen.curri_id,"aca_year":$scope.year_choosen.aca_year}

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
            alertCaller.success();
            $scope.close_modal(my_modal);
        }).
        error(function (data, status, headers, config) {
            $scope.please_wait = false;
            alertCaller.error(null,null,data.message);
        });
    };
});

app.controller('manage_president_controller', function($scope, $alert,$http,request_years_from_curri_choosen_service,alertCaller) {
$scope.init =function() {
     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {};
                $scope.results={};
                $scope.my_president ={};
                       $scope.current_president = {};
                    
                 $scope.old_pres_ob = {};
                               $http.get('/api/curriculumacademic/getdistinctacayear').success(function (data) {
           $scope.corresponding_aca_years =data;
          });
}

$scope.add_president = function(curri_key){
    $scope.results.all_presidents[curri_key].presidents.push({'tname':"",'pic':'','email':''});
}

$scope.change = function(curri_key){
}
$scope.remove_president = function(curri_key,index_to_remove){
     $scope.results.all_presidents[curri_key].presidents.splice(index_to_remove, 1);     
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
                $scope.results={};
                $scope.my_president ={};
                $scope.current_president= {};

$scope.nothing_change_object = function(){
      if(!$scope.old_pres_ob){
        return true;
      }
      if(!$scope.results){
        return true;
      }
      if(angular.equals($scope.old_pres_ob,$scope.results)==true){
        return true;
      }
      return false;
}
    $scope.find_information = function(){
        $http.post(
             '/api/presidentcurriculum/getallpres',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
              $scope.results = data;
            $scope.choose_not_complete = false;
            var index;
            for(index=0;index<$scope.results.all_curri_id.length;index++){
                var index2;
       var new_presidents = [];
                for(index2=0;index2<$scope.results.all_presidents[$scope.results.all_curri_id[index]].presidents.length;index2++){             
                    var index3;
                    for(index3=0;index3<$scope.results.all_presidents[$scope.results.all_curri_id[index]].candidates.length;index3++){
                        if($scope.results.all_presidents[$scope.results.all_curri_id[index]].candidates[index3].tname == $scope.results.all_presidents[$scope.results.all_curri_id[index]].presidents[index2].tname){
                            new_presidents.push($scope.results.all_presidents[$scope.results.all_curri_id[index]].candidates[index3]);
                        }
                    } 
                }
                  $scope.results.all_presidents[$scope.results.all_curri_id[index]].presidents = new_presidents;
            }
$scope.old_pres_ob = angular.copy($scope.results);
         });
    }

    $scope.choose_same_presidents_or_not_choose = function(){
        if(!$scope.results){
            return true;
        }
           if(!$scope.results.all_curri_id){
            return true;
        }
            var index;
            for(index=0;index<$scope.results.all_curri_id.length;index++){
                var index2;
       var backup_presidents = [];
                for(index2=0;index2<$scope.results.all_presidents[$scope.results.all_curri_id[index]].presidents.length;index2++){
                if(!$scope.results.all_presidents[$scope.results.all_curri_id[index]].presidents[index2].tname){
                    return true;
                }
               if(backup_presidents.indexOf($scope.results.all_presidents[$scope.results.all_curri_id[index]].presidents[index2])==-1)
               {
                 backup_presidents.push($scope.results.all_presidents[$scope.results.all_curri_id[index]].presidents[index2]);
               }
               else{
                return true;
               }                  
                }
            }
            return false;
    }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }

    $scope.save_to_server = function(my_modal){
        $scope.new_obj_to_send ={};

        $scope.new_obj_to_send.old_object = $scope.results;

        $scope.new_obj_to_send.aca_year = $scope.year_choosen;
        $http.put(
             '/api/presidentcurriculum/saveallpres',
             JSON.stringify($scope.new_obj_to_send),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             alertCaller.success();
             $scope.close_modal(my_modal);
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  }); 
    }
});
 
app.controller('manage_admin_add_admin_controller', function($scope, $rootScope,$alert,$http,request_years_from_curri_choosen_service,alertCaller) {
$scope.init =function() {
    $scope.new_admin = {};
    $scope.new_admin.t_name = "";
    $scope.new_admin.email = "";
}
$scope.init();
    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
    });
  $scope.$on("modal.show", function (event, args) {
        $scope.init();
    });
      $scope.close_modal = function(my_modal){
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
           $http.get('/api/admin').success(function (data) {
             $rootScope.all_admins = data;
           });
           alertCaller.success(null,'เพิ่มข้อมูลเรียบร้อย ตรวจสอบอีเมล์เพื่อรับรหัสผ่าน');
               my_modal.$hide();
    })
             .error(function(data, status, headers, config) {
                 alertCaller.error(null,null,data.message);
  }); 
    }
});

app.controller('manage_admin_who_controller', function($scope, $rootScope,$alert,$http,request_years_from_curri_choosen_service,Lightbox,alertCaller) {
$scope.init =function() {   
                       $scope.email_new_admin = "";

   $http.get('/api/admin').success(function (data) {
             $rootScope.all_admins = data;
           });
}

$scope.email_new_admin = "";

 $scope.openLightboxModal = function (to_open) {
    $scope.fake_array = [];
    $scope.fake_array.push(to_open);
    Lightbox.openModal( $scope.fake_array, 0);
  };

    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
    });
  $scope.$on("modal.show", function (event, args) {
        $scope.init();
    });
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
});

app.controller('manage_indicators_controller', function($scope, $alert,$http,$rootScope,$modal,alertCaller){
$scope.init = function(){
     $scope.choose_not_complete = true;
     $scope.year_choosen = 0;
      $rootScope.manage_indicators_and_subs_year_choosen = 0;
      $scope.please_wait = false;
$scope.nothing_change = true;
      $scope.curri_choosen = {};
$scope.have_thai_name = false;
    $rootScope.my_backup_indicators = {};
$rootScope.manage_indicators_year_to_create = "";
    $scope.year_to_create = "";

      $http.get('/api/curriculumacademic/getmaxacayear').success(function (data) {
             $scope.max_year_curri_have = data;
           });
   $http.get('/api/indicator').success(function (data) {
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

    $scope.year_to_create = ""; 
    $scope.choose_not_complete = true;
    $scope.year_choosen = 0;
    $rootScope.manage_indicators_and_subs_year_choosen = 0;
    $scope.curri_choosen = {};
    $rootScope.my_backup_indicators = {};

    $rootScope.manage_indicators_year_to_create = "";
      $scope.recover_indicator_all =function(){
            $rootScope.manage_indicators_and_sub_result = angular.copy($rootScope.my_backup_indicators);
      }

        $scope.validate_year_to_create = function(){       
            return   angular.isUndefined($scope.year_to_create) || $scope.year_to_create < $scope.max_year_curri_have || $scope.year_to_create == "" ;
        }
      $scope.choose_indicator = function(in_indi){

if( $scope.validate_year_to_create() != true){

        $rootScope.manage_indicators_and_sub_save_indicator = {};
        $rootScope.manage_indicators_and_sub_save_indicator.save_content = [];
        $rootScope.manage_indicators_and_sub_save_indicator.save_index = 0;
      
            $rootScope.manage_indicators_indicator_choosen = in_indi;
           $rootScope.manage_indicators_and_sub_save_indicator.save_content =  angular.copy(in_indi);
           
            $rootScope.manage_indicators_and_sub_save_indicator.save_index = angular.copy($rootScope.manage_indicators_and_sub_result.indexOf(in_indi));
       
         $rootScope.manage_indicators_year_to_create = $scope.year_to_create;
         $rootScope.world_have_thai_name = $scope.have_thai_name;
         }
         else{
             alertCaller.error(null,'กรุณากรอกปีการศึกษาให้ถูกต้อง');  
         }
      }
              $scope.still_not_choose_complete =function(){
                  if(!$rootScope.manage_indicators_and_sub_result){
                return true;
            }
                var index;
                $scope.my_num_indicators = [];
                for (index =0;index<  $rootScope.manage_indicators_and_sub_result.length ; index++){
                    if (!$rootScope.manage_indicators_and_sub_result[index].indicator_num || !$rootScope.manage_indicators_and_sub_result[index].indicator_name_e ){
                        return true;
                    }
                    if($scope.have_thai_name==true){
                                console.log('have_thai_name')
                        if(!$rootScope.manage_indicators_and_sub_result[index].indicator_name_t){
                            console.log('blank')
                            return true;
                        }
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
         $rootScope.manage_indicators_and_sub_result.push({"sub_indicator_list":[]
        ,"aca_year":$rootScope.year_to_create
        ,"indicator_num":""
        ,"indicator_name_t":"","indicator_name_e":""});
      }

      $scope.remove_indicator = function(index_indicator_to_remove) { 
              $scope.nothing_change = false;
      $rootScope.manage_indicators_and_sub_result.splice(index_indicator_to_remove, 1);
    }

    $scope.get_indicators = function(){
          $rootScope.manage_indicators_and_subs_year_choosen = $scope.year_choosen;
$scope.choose_not_complete = false;

          $http.post(
             '/api/indicatorsubindicator',
             JSON.stringify($rootScope.manage_indicators_and_subs_year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.nothing_change = true;
             $rootScope.my_backup_indicators = angular.copy(data);
            $rootScope.manage_indicators_and_sub_result =data;

             var index;
                $scope.my_num_indicators = [];
                var not_have_thai = true;
                for (index =0;index<  $rootScope.manage_indicators_and_sub_result.length ; index++){
                     if($rootScope.manage_indicators_and_sub_result[index].indicator_name_t!=""){
                            not_have_thai = false;
                        }
                }
                if(not_have_thai == false){
                    $scope.have_thai_name =true; 
                }
                else{
                    $scope.have_thai_name =false; 
                }
         });
    }

    $scope.something_change = function(){
        $scope.nothing_change = false;
    }
    $scope.close_modal = function(my_modal){
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

    if($scope.have_thai_name ==false){
        $rootScope.manage_indicators_and_sub_result[index].indicator_name_t = "";
    }
 }

if ($rootScope.manage_indicators_and_sub_result.length == 0){
    $rootScope.manage_indicators_and_sub_result = [];  
    $rootScope.manage_indicators_and_sub_result.push({'aca_year':$scope.year_to_create});
}
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
            alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  }); 
    }
});

app.controller('change_evidence_file_controller', function($scope, $alert,$http,request_years_from_curri_choosen_service,$rootScope,fileChecker,alertCaller) {
       $scope.init =function() {
    $scope.please_wait = false;
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
        $scope.$apply(function () {
            $scope.files = [];
            if (!fileChecker.stdFileChk(args.file)) {
                fileChecker.resetFileInput('');
            }
            else {
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

        $http({
            method: 'PUT',
            url: "/api/evidence/updateevidencefile",
            headers: { 'Content-Type': undefined },
            data:formData,
            transformRequest: angular.indentity 
        }).
        success(function (data, status, headers, config) {
              $scope.please_wait = false;
    $rootScope.manage_evidences_still_same();
            $rootScope.manage_evidences_world_evidences = data;
            $scope.close_modal(this_modal);
            alertCaller.success();
        }).
        error(function (data, status, headers, config) {
                      $scope.please_wait = false;
                      alertCaller.error(null,null,data.message);
        });
    }
});

app.controller('add_new_evidence_controller', function($scope, $alert,$http,$rootScope,request_years_from_curri_choosen_service,fileChecker,alertCaller){
 
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
      
        var index;
  $rootScope.my_evidence_real_code_we_have_now = [];
  $rootScope.my_evidence_name_we_have_now =[];
             for(index=0;index<$rootScope.manage_evidences_world_evidences.length;index++){
                $rootScope.my_evidence_real_code_we_have_now.push($rootScope.manage_evidences_world_evidences[index].evidence_real_code);
                 $rootScope.my_evidence_name_we_have_now.push($rootScope.manage_evidences_world_evidences[index].evidence_name);
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
        $scope.$apply(function () {
            $scope.my_new_evidence_file = [];
            if (!fileChecker.stdFileChk(args.file)) {
                fileChecker.resetFileInput('');
            }
            else {
                $scope.my_new_evidence_file.push(args.file);
            }
        });
    });
    $scope.new_evidence_still_not_complete =  function(){
        if (!$scope.my_new_evidence.evidence_real_code || !$scope.my_new_evidence.evidence_name|| $scope.my_new_evidence_file.length ==0){
                return true;
        }
        else{
             if ($scope.my_new_evidence.evidence_real_code <= 0 ){
                   return true;
                }
            if($rootScope.my_evidence_real_code_we_have_now.indexOf($scope.my_new_evidence.evidence_real_code) != -1){
                   return true;
            }
            if($rootScope.my_evidence_name_we_have_now.indexOf($scope.my_new_evidence.evidence_name) != -1){
                return true;
            }
            return false;
        }
    }

 $scope.close_modal = function(my_modal){
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


  $scope.to_sent = {};
$scope.to_sent.my_new_evidence = $scope.my_new_evidence;

$scope.to_sent.all_evidences = $rootScope.manage_evidences_world_evidences;
    
    formData.append("model", angular.toJson(  $scope.to_sent));
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
               $scope.close_modal(my_modal);
               alertCaller.success();      
        }).
        error(function (data, status, headers, config) {
                      $scope.please_wait = false;
                      alertCaller.error(null,null,data.message);
        });
    }
});

app.controller('add_new_primary_controller', function($scope, $alert,$http,$rootScope,request_years_from_curri_choosen_service,fileChecker,alertCaller){
    $scope.my_new_evidence = {};
   $scope.my_new_evidence.evidence_real_code = "";
   $scope.my_new_evidence.evidence_name = "";
   $scope.my_new_evidence.secret = false;
   $scope.my_new_evidence_file = [];
 $scope.my_temp_secret_new = false;

$scope.primary_choosen = {};
        $scope.please_wait = false;
    $scope.init =function() {

           angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
           
        $scope.please_wait = false;
         $scope.my_temp_secret_new = false;
   $scope.my_new_evidence = {};
   $scope.my_new_evidence.evidence_real_code = "";
   $scope.my_new_evidence.evidence_name = "";
   $scope.my_new_evidence_file = [];
     $scope.my_new_evidence.secret = false;

       $scope.primary_choosen = {};

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
       $scope.$apply(function () {
           $scope.my_new_evidence_file = [];
           if (!fileChecker.stdFileChk(args.file)) {
               fileChecker.resetFileInput('');
           }
           else {
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

$scope.primary_choosen.file_name = $scope.my_new_evidence_file[0].name;
$scope.primary_choosen.curri_id =   $rootScope.manage_evidence_curri_id_now;
    $scope.primary_choosen.aca_year = $rootScope.manage_evidence_year_now;
    $scope.primary_choosen.indicator_num = $rootScope.manage_evidence_indicator_num;
$scope.primary_choosen.secret = $scope.my_new_evidence.secret;
$scope.primary_choosen.evidence_real_code =   $scope.my_new_evidence.evidence_real_code;
$scope.primary_choosen.teacher_id = $rootScope.current_user.user_id;

$scope.to_sent = {};
$scope.to_sent.primary_choose = $scope.primary_choosen;
$scope.to_sent.all_evidences = $rootScope.manage_evidences_world_evidences;

    formData.append("model", angular.toJson($scope.to_sent));
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
               alertCaller.success();       
        }).
        error(function (data, status, headers, config) {
                      $scope.please_wait = false;
                      alertCaller.error(null,null,data.message);
        });
    }
});

app.controller('manage_evidences_controller', function($scope, $alert,$http,$rootScope,request_years_from_curri_choosen_service,alertCaller,$filter,$q){

        $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {}
                $scope.indicator_choosen= {};
$rootScope.my_evidence_real_code_we_have_now =[];

$rootScope.my_all_primary_evidences_responsible = [];
$scope.nothing_change =true;

$rootScope.manage_evidences_still_same = function(){
    $scope.nothing_change =true;
}

 $scope.$on("modal.hide", function (event, args) {
     $scope.init();
    });
  $scope.$on("modal.show", function (event, args) {
    $scope.init();
    var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
    if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.sendCurriAndGetYears().then(function(){
            var target_aca_year = $filter('filter')($scope.corresponding_aca_years, {aca_year: $rootScope.current_aca_year},true);
            if(target_aca_year.length > 0){
                $scope.year_choosen = target_aca_year[0];
                $scope.find_indicators().then(function(){
                    if(!!$rootScope.current_indicator){
                        var target_indicator = $filter('filter')($scope.corresponding_indicators, {aca_year: $rootScope.current_indicator.aca_year,indicator_num: $rootScope.current_indicator.indicator_num},true);
                        if(target_indicator.length > 0){
                            $scope.indicator_choosen = target_indicator[0];
                            $scope.find_information();
                        }
                    }
                });
            }
        });
    }
    });

   $scope.remove_evidence = function(index_evidence_to_remove) {
    if( ! $rootScope.manage_evidences_world_evidences[index_evidence_to_remove].primary_evidence_num || $rootScope.manage_evidences_world_evidences[index_evidence_to_remove].primary_evidence_num <=0){
            $rootScope.manage_evidences_world_evidences.splice(index_evidence_to_remove, 1);
            $scope.nothing_change =false;
    }
else{
    alertCaller.error(null,'ไม่สามารถลบหลักฐานพื้นฐานได้');
    }
}

    $scope.something_change= function(){
        $scope.nothing_change = false;
    }

$scope.still_not_choose_complete = function(){
     if(!$rootScope.manage_evidences_world_evidences){
        return true;
    }
  var index;
  $scope.all_evidence_real_code = [];
    $scope.all_evidence_name = [];
  for(index = 0; index < $rootScope.manage_evidences_world_evidences.length ;index++){
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
        if( $scope.all_evidence_name.indexOf($rootScope.manage_evidences_world_evidences[index].evidence_name)!=-1){
            return true;
        }
        $scope.all_evidence_name.push($rootScope.manage_evidences_world_evidences[index].evidence_name);
        $scope.all_evidence_real_code.push($rootScope.manage_evidences_world_evidences[index].evidence_real_code);
    }
  }
return false;
}
    $scope.watch_file = function(path) { 
        window.open(path, '_blank', "width=800, left=230,top=0,height=700");  
    }

$scope.choose_to_change_file = function(this_obj){
    $rootScope.only_object_want_to_change = angular.copy(this_obj);
    $rootScope.manage_evidence_indicator_num = $scope.indicator_choosen.indicator_num;
 if($rootScope.only_object_want_to_change.secret == 1){
    $rootScope.only_object_want_to_change.secret = true;
   }
   else{
     $rootScope.only_object_want_to_change.secret = false;
   }
}

$scope.choose_to_add_new_file = function(){    
    $rootScope.manage_evidence_indicator_num = $scope.indicator_choosen.indicator_num;
    $rootScope.manage_evidence_curri_id_now = $scope.curri_choosen.curri_id;
    $rootScope.manage_evidence_year_now = $scope.year_choosen.aca_year;
}

$scope.choose_to_add_new_primary_file = function(){
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
             $rootScope.my_all_primary_evidences_responsible = data;          
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
                    $rootScope.my_evidence_real_code_we_have_now =[];
                    $scope.nothing_change =true;
                    $scope.all_curri_that_have_privileges = [];
                    $scope.corresponding_aca_years = [];
                 $scope.corresponding_indicators = [];
  $scope.$parent.scan_only_privilege_curri('3',$scope.all_curri_that_have_privileges);
}

    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
       $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
        $scope.indicator_choosen= {};
      $scope.nothing_change =true;
        var promise = request_years_from_curri_choosen_service.async($scope.curri_choosen,3,2).then(function(data) {
            $scope.corresponding_aca_years = data;
            $scope.corresponding_indicators = [];
            return $q.resolve("");
          });
          return promise;
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
        $scope.indicator_choosen = {};
        var promise = $http.post(
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
              return $q.resolve("");
         });
         return promise;
    }

    $scope.save_to_server = function(my_modal){
        if($rootScope.manage_evidences_world_evidences.length ==0){
            $scope.to_sent = {};
             $scope.to_sent.curri_id = $scope.curri_choosen.curri_id;
             $scope.to_sent.aca_year = $scope.year_choosen.aca_year;
            $scope.to_sent.indicator_num = $scope.indicator_choosen.indicator_num;
             $scope.to_sent.evidence_code = 0;
             $rootScope.manage_evidences_world_evidences.push( $scope.to_sent);
        }
        $http.put(
             '/api/evidence/updateevidence',
             JSON.stringify($rootScope.manage_evidences_world_evidences),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
              $scope.find_information();
              alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  }); 
    }
});

app.controller('manage_sub_indicators_controller', function($scope, $alert,$http,$rootScope,$modalBox,alertCaller){
$scope.please_wait = false;
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
       $rootScope.manage_indicators_and_sub_result[$rootScope.manage_indicators_and_sub_save_indicator.save_index] = {};
     
       $rootScope.manage_indicators_indicator_choosen = {};

       $rootScope.manage_indicators_indicator_choosen = angular.copy($rootScope.manage_indicators_and_sub_save_indicator.save_content);
         $rootScope.manage_indicators_and_sub_result[$rootScope.manage_indicators_and_sub_save_indicator.save_index] =  angular.copy($rootScope.manage_indicators_indicator_choosen);
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
    }
        $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
$scope.start_ka = function(){
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
       
        $scope.to_sent = angular.copy($rootScope.manage_indicators_and_sub_result);
        $scope.to_sent[$rootScope.manage_indicators_and_sub_save_indicator.save_index] = angular.copy($rootScope.manage_indicators_indicator_choosen);

                var index;
          var sub_index;
 for (index = 0; index < $scope.to_sent.length; index++) {
    $scope.to_sent[index].aca_year = $rootScope.manage_indicators_year_to_create;

    if(angular.isUndefined($scope.to_sent[index].sub_indicator_list)){
        $scope.to_sent[index].sub_indicator_list = [];
    }
    
    if($rootScope.world_have_thai_name == false){
        $scope.to_sent[index].indicator_name_t = "";
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
             alertCaller.success();
                $rootScope.manage_indicators_and_sub_result = angular.copy( $scope.to_sent);
                $rootScope.my_backup_indicators= angular.copy( $scope.to_sent);
                $rootScope.manage_indicators_and_sub_save_indicator.save_content = angular.copy($rootScope.manage_indicators_indicator_choosen);
                $scope.close_modal(my_modal);
         })
    .error(function(data, status, headers, config) {
$scope.please_wait = false;
alertCaller.error(null,null,data.message);
  }); 
    }
});

app.controller('manage_primary_evidences_admin_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller) {

$scope.init =function() {
     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
               $scope.indicator_choosen= {};
                 $scope.corresponding_indicators = [];
                 $scope.nothing_change = true;
                   $scope.go_request = false;
        $http.get('/api/curriculumacademic/getdistinctacayear').success(function (data) {
           $scope.corresponding_aca_years =data;
          });
}
$scope.init();
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
         $scope.result.push({ "evidence_name":"","just_create":true,"curri_id":"0","aca_year":$scope.year_choosen,"indicator_num":$scope.indicator_choosen.indicator_num
});
      }

    $scope.remove_primary_evidence = function(index_primary_evidence_to_remove) { 
              $scope.nothing_change = false;
      $scope.result.splice(index_primary_evidence_to_remove, 1);     

    }
  $scope.find_indicators = function(){
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
        my_modal.$hide();
    }
      $scope.save_to_server = function(my_modal){
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
             alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  }); 
    }
});

app.controller('manage_primary_evidences_president_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller,$filter,$q) {
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
    var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
    if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.sendCurriAndGetYears().then(function(){
            var target_aca_year = $filter('filter')($scope.corresponding_aca_years, {aca_year: $rootScope.current_aca_year},true);
            if(target_aca_year.length > 0){
                $scope.year_choosen = target_aca_year[0];
                $scope.find_indicators().then(function(){
                    if(!!$rootScope.current_indicator){
                        var target_indicator = $filter('filter')($scope.corresponding_indicators, {aca_year: $rootScope.current_indicator.aca_year,indicator_num: $rootScope.current_indicator.indicator_num},true);
                        if(target_indicator.length > 0){
                            $scope.indicator_choosen = target_indicator[0];
                            $scope.find_information();
                        }
                    }
                });
            }
        });
    }
    });

     $scope.choose_not_complete = true;
        $scope.year_choosen = {};
              $scope.curri_choosen = {};
               $scope.indicator_choosen= {};
 $scope.corresponding_aca_years = [];
       $scope.corresponding_indicators = {};
     $scope.nothing_change = true;

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
        $scope.year_choosen = {};
        $scope.indicator_choosen= {};
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
         var promise = request_years_from_curri_choosen_service.async($scope.curri_choosen,999).then(function(data) {
                    $scope.corresponding_indicators = [];
                    $scope.corresponding_aca_years = data;
                    $scope.nothing_change = true;
                $scope.choose_not_complete = true;
                return $q.resolve("");
         });
          return promise;
    }

     $scope.add_primary_evidence = function(){
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
primary_obj.wait_send_email = true;
if(angular.isUndefined(primary_obj.teacher_id)){
    alertCaller.error(null,'กรุณาเลือกผู้รับผิดชอบหลักฐานก่อนส่ง');
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
            primary_obj.wait_send_email = false;
            alertCaller.success(null,'ส่ง Email แจ้งเตือนเรียบร้อย');
         })
    .error(function(data, status, headers, config) {
             primary_obj.wait_send_email = false;
             alertCaller.error(null,'ส่ง Email แจ้งเตือนไม่สำเร็จ');
  });
}
}
      $scope.remove_primary_evidence = function(index_primary_evidence_to_remove) {
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
        my_modal.$hide();
    }

  $scope.find_indicators = function(){
$scope.indicator_choosen = {};
        var promise = $http.post(
             '/api/indicator/querybycurriculumacademic',
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
            return $q.resolve("");
         });
         return promise;
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
             $scope.result = data;
             $scope.choose_not_complete = false;
             $scope.nothing_change = true;
         });
    }
$scope.dont_show_me =function(my_obj){
    if(my_obj.status == '3' || my_obj.status == '7'){
        return true;
    }
    else{
        return false;
    }
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
               alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  }); 
    }
});

app.controller('result_survey_controller', function($scope, $http,$alert,$loading,$rootScope) {
  $scope.$on("modal.show", function (event, args) {

      if($rootScope.no_open_result_survey == true){
        $rootScope.no_open_result_survey = false;

     args.hide()
        }              
    });
});

app.controller('answer_survey_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller) {
$scope.init =function() {
                    $scope.suggestion = "";
                    $scope.please_wait = false;
}
       $scope.$on("modal.hide", function (event, args) { 
     $scope.init();
    });

  $scope.$on("modal.show", function (event, args) {
      if($rootScope.no_open_answer_survey == true){
        $rootScope.no_open_answer_survey =false;

     args.hide()
        }
        else{
            $scope.init();
        }     
    });
 $scope.suggestion = "";
 $scope.still_not_complete=function(){
    if(! $scope.suggestion){
        return true;
    }

    var index;
    for(index =0 ;index< $rootScope.manage_survey_questionare_set.length ;index++){
        if (!$rootScope.manage_survey_questionare_set[index].answer){
            return true;
        }
    }
 }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
  $scope.save_to_server = function(my_modal){
     $scope.please_wait = true;
        $scope.to_sent = angular.copy( $rootScope.manage_survey_questionare_set);
         $scope.to_sent.push({"suggestion":$scope.suggestion});
        $http.put(
             '/api/questionareanswer',
             JSON.stringify( $scope.to_sent),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.close_modal(my_modal);
            alertCaller.success();
         })
    .error(function(data, status, headers, config) {
         $scope.please_wait = false;
         alertCaller.error(null,null,data.message);
  }); 
    }
});

app.controller('manage_lab_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller,$filter,$q) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {};
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
    var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
    if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.sendCurriAndGetYears().then(function(){
            var target_aca_year = $filter('filter')($scope.corresponding_aca_years, {aca_year: $rootScope.current_aca_year},true);
            if(target_aca_year.length > 0){
                $scope.year_choosen = target_aca_year[0];
                $scope.find_information();
            }
        });
    }
    });

     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {};
  $scope.nothing_change = true;

 $rootScope.manage_lab_my_world_wide_labs = [];
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {};
         $scope.nothing_change = true;
      
            var promise = request_years_from_curri_choosen_service.async($scope.curri_choosen,10,2).then(function(data) {
            $scope.corresponding_aca_years = data;
            return $q.resolve("");
            });
          return promise;
    }

    $scope.go_to_create_lab =function(){
        $rootScope.lab_ctrl_mode = 1;
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

      $scope.go_to_fix_lab = function(lab_to_fix){
          $rootScope.lab_ctrl_mode = 2;
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
             $rootScope.manage_lab_fix_this_lab = angular.copy(lab_to_fix);
             $rootScope.manage_lab_fix_this_lab.officer = [];
             var index;
             var inside_index;
        for(index =0;index<lab_to_fix.officer.length;index++){
            for(inside_index=0;inside_index<$rootScope.manage_lab_all_personnels_in_curri.length;inside_index++){
                if($rootScope.manage_lab_all_personnels_in_curri[inside_index].user_id  == lab_to_fix.officer[index].user_id){
                          $rootScope.manage_lab_fix_this_lab.officer.push($rootScope.manage_lab_all_personnels_in_curri[inside_index]);break;
                }
            }
        }
         });
    }

    $scope.remove_lab = function(index_to_remove){
        $scope.nothing_change = false;
        $rootScope.manage_lab_my_world_wide_labs.splice(index_to_remove, 1);   
    }

    $scope.find_information = function(){
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
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
        if($rootScope.manage_lab_my_world_wide_labs.length == 0){
            $scope.to_sent  = {};
            $scope.to_sent.curri_id  = $scope.curri_choosen.curri_id;
            $scope.to_sent.aca_year = $scope.year_choosen.aca_year;
            $rootScope.manage_lab_my_world_wide_labs.push($scope.to_sent);
        }
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
            alertCaller.success();    
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  }); 
    }
});

app.controller('manage_survey_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller,$filter,$q) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {};
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
        var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
    if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.sendCurriAndGetYears().then(function(){
            var target_aca_year = $filter('filter')($scope.corresponding_aca_years, {aca_year: $rootScope.current_aca_year},true);
            if(target_aca_year.length > 0){
                $scope.year_choosen = target_aca_year[0];
                $scope.find_information();
            }
        });
    }
    });
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {};
  $scope.nothing_change = true;

  $rootScope.manage_survey_my_world_wide_surveys = [];
        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {};
         $scope.nothing_change = true;
      
        var promise = request_years_from_curri_choosen_service.async($scope.curri_choosen,5,2).then(function(data) {
            $scope.corresponding_aca_years = data;
            return $q.resolve("");
          });
        return promise;
    }

$scope.right_target = function(targets){
    var i;
    for(i= 0 ;i<targets.length;i++ ){
        if( $rootScope.current_user.user_type == targets[i].user_type){
            return true;
        }

        if(targets[i].user_type == 'กรรมการหลักสูตร' && $rootScope.current_user.committee_in){
            if($rootScope.current_user.committee_in[$scope.curri_choosen.curri_id]){
                var index;
                for(index=0;index<$rootScope.current_user.committee_in[$scope.curri_choosen.curri_id].length;index++){
                    if($rootScope.current_user.committee_in[$scope.curri_choosen.curri_id][index] == $scope.year_choosen.aca_year){
                        return true;
                    }
                }
            }
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
         })        
         .error(function(data, status, headers, config) {
            $rootScope.no_open_answer_survey =true;
            alertCaller.error(null,data.message);
  });
        $rootScope.manage_survey_questionare_to_answer = this_survey;
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
         })   .error(function(data, status, headers, config) {
           $rootScope.no_open_result_survey = true;
           alertCaller.error(null,data.message);
  }); 
        $rootScope.manage_survey_questionare_of_result = this_survey;
    }

    $scope.remove_qestionare = function(index_to_remove){
        $scope.nothing_change = false;
        $rootScope.manage_survey_my_world_wide_surveys.splice(index_to_remove, 1);   
    }
    $scope.go_to_create_survey = function(){
        $rootScope.manage_survey_curri_id_now = $scope.curri_choosen.curri_id;
        $rootScope.manage_survey_aca_year_now = $scope.year_choosen.aca_year;
    }

    $scope.find_information = function(){
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
             $scope.choose_not_complete = false;
         });
    }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
        if($rootScope.manage_survey_my_world_wide_surveys.length == 0){
            $scope.to_sent  = {};
            $scope.to_sent.curri_id  = $scope.curri_choosen.curri_id;
            $scope.to_sent.aca_year = $scope.year_choosen.aca_year;
            $scope.to_sent.questionare_set_id = 0;
            $rootScope.manage_survey_my_world_wide_surveys.push($scope.to_sent);
        }

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
            alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  });
    }
});

app.controller('show_edit_album_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,Lightbox,fileChecker,alertCaller) {
  $scope.openLightboxModal = function (index) {
    Lightbox.openModal($rootScope.manage_album_show_this_album.pictures, index);
  }

  $scope.my_pictures = {};
        $scope.please_wait = false;
  $scope.close_modal = function(my_modal){
            $scope.please_wait = false;
        $scope.my_pictures.flow.cancel();
        my_modal.$hide();
    }

  $scope.$on("modal.show", function (event, args) {
      $scope.please_wait = false;
      $scope.my_pictures.flow.cancel();
    });

  $scope.remove_pic = function(index_to_remove){
        $rootScope.manage_album_show_this_album.pictures.splice(index_to_remove,1);
  }
$scope.imgFileCheck = function(file){
    return fileChecker.imgFileChk(file);
}
    $scope.still_not_choose_complete = function(){
               if(!$scope.my_pictures.flow){
            return true;
        }
        if(!$rootScope.manage_album_show_this_album){
            return true;
        }
        if(!$rootScope.manage_album_show_this_album.name){
            return true;
        }
        else{
                if($scope.my_pictures.flow.files.length ==0 && $rootScope.manage_album_show_this_album.pictures.length ==0){
                    return true;
                }
          var index;
          for(index = 0 ;index < $rootScope.manage_album_show_this_album.pictures.length ;index++){
            if(!$rootScope.manage_album_show_this_album.pictures[index].caption){
                return true;
            }
          }

            for(index = 0 ;index < $scope.my_pictures.flow.files.length ;index++){
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

        $http({
            method: 'PUT',
            url: "/api/gallery/edit",
            headers: { 'Content-Type': undefined },
            data:formData,
            transformRequest: angular.indentity
        }).
        success(function (data, status, headers, config) {
               $scope.please_wait = false;
        $rootScope.manage_album_still_same();
                $rootScope.manage_album_my_world_wide_album =data;
                $scope.close_modal(my_modal);
                alertCaller.success();
        }).
        error(function (data, status, headers, config) {
                       $scope.please_wait = false;
                       alertCaller.error(null,null,data.message);
        });
    }
});

app.controller('show_album_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,Lightbox) {
  $scope.openLightboxModal = function (index) {
    Lightbox.openModal($rootScope.manage_album_show_this_album.pictures, index);
  }
});

app.controller('manage_album_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,Lightbox,alertCaller,$filter,$q) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {};
                    $scope.nothing_change = true;
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
    var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
    if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.sendCurriAndGetYears().then(function(){
            var target_aca_year = $filter('filter')($scope.corresponding_aca_years, {aca_year: $rootScope.current_aca_year},true);
            if(target_aca_year.length > 0){
                $scope.year_choosen = target_aca_year[0];
                $scope.find_information();
            }
        });
    }
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
              $scope.curri_choosen = {};
  $scope.nothing_change = true;

        $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {};
         $scope.nothing_change = true;
      
        var promise = request_years_from_curri_choosen_service.async($scope.curri_choosen,6,2).then(function(data) {
            $scope.corresponding_aca_years = data;
            return $q.resolve("");
        });
        return promise;
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
    }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
        if($rootScope.manage_album_my_world_wide_album.length == 0){
         
            $scope.to_sent  = {};
            $scope.to_sent.curri_id  = $scope.curri_choosen.curri_id;
            $scope.to_sent.aca_year = $scope.year_choosen.aca_year;
      
            $rootScope.manage_album_my_world_wide_album.push($scope.to_sent);
        }
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
            alertCaller.success();         
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  });
    }
});

app.controller('import_evidence_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller) {
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
                   $alert({title:'เกิดข้อผิดพลาด', content:'รหัสหลักฐานซ้ำกับรหัสหลักฐานที่มีอยู่แล้ว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
        }

        if($rootScope.my_evidence_name_we_have_now.indexOf($scope.evidence_we_want.evidence_name) != -1){
  return true;
                   $alert({title:'เกิดข้อผิดพลาด', content:'หลักฐานที่เลือกมีชื่อซ้ำกับหลักฐานที่มีอยู่แล้ว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
        }
          return false;
    }
}
$scope.watch_preview = function(){
    if($scope.choose_not_complete == false ){
        $scope.watch_file($scope.evidence_we_want.file_name);
    }
    else{
        alertCaller.error(null,'กรุณาเลือกหลักฐานที่ต้องการดู');
    }
}

$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {};
   $scope.evidence_we_want = "";
                    $scope.code_we_want = "";
$scope.corresponding_aca_years = [];
$scope.all_evidences =[];
 $scope.all_curri_that_have_privileges = [];
  $scope.$parent.scan_only_privilege_curri('3',$scope.all_curri_that_have_privileges);

 var index;
  $rootScope.my_evidence_real_code_we_have_now = [];
  $rootScope.my_evidence_name_we_have_now = [];
             for(index=0;index<$rootScope.manage_evidences_world_evidences.length;index++){
                $rootScope.my_evidence_real_code_we_have_now.push($rootScope.manage_evidences_world_evidences[index].evidence_real_code);
                $rootScope.my_evidence_name_we_have_now.push($rootScope.manage_evidences_world_evidences[index].evidence_name);
             }
                }
  
   $scope.$on("modal.hide", function (event, args) {
     $scope.init();
    });

$scope.choose_evidence = function(){
        $scope.choose_not_complete = false;
}
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
 $scope.choose_not_complete = true;
$scope.all_evidences =[];
            $scope.corresponding_aca_years = data;
   $scope.evidence_we_want = {};
          });
    }

    $scope.find_all_evidences_in_curri_and_year = function(){
  $scope.evidence_we_want = {};
        
 $scope.choose_not_complete = true;
        $http.post(
             '/api/evidence/getbycurriculumacademic',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
              $scope.all_evidences = data;
         });
    }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){

$scope.evidence_we_want.curri_id =   $rootScope.manage_evidence_curri_id_now;
$scope.evidence_we_want.aca_year = $rootScope.manage_evidence_year_now;
$scope.evidence_we_want.indicator_num = $rootScope.manage_evidence_indicator_num;
$scope.evidence_we_want.evidence_real_code = $scope.code_we_want;
$scope.evidence_we_want.teacher_id = $rootScope.current_user.user_id;

        $scope.to_sent = {};
        $scope.to_sent.evidence_import = $scope.evidence_we_want;
        $scope.to_sent.all_evidences = $rootScope.manage_evidences_world_evidences;

        $http.put(
             '/api/evidence/newevidencefromothers',
             JSON.stringify( $scope.to_sent ),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $rootScope.manage_evidences_still_same();
             $rootScope.manage_evidences_world_evidences = data;
              $scope.close_modal(my_modal);
              alertCaller.success();          
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  });
    }
});

app.controller('create_user_controller', function($scope, $http,$alert,$loading,$rootScope,fileChecker,alertCaller) {
  $scope.init = function(){
      angular.forEach(
          angular.element("input[type='file']"),
          function (inputElem) {
              angular.element(inputElem).val(null);
          });
      $scope.files = [];
      $scope.please_wait = false;
      $scope.new_user_obj = {};
      $scope.new_user_obj.curri = [];
      $scope.new_user_obj.type = '';
      $scope.new_user_obj.new_emails = [];
      if ($rootScope.current_user.user_type != 'ผู้ดูแลระบบ') {
          $scope.all_curri_that_have_privileges = [];
          $scope.$parent.scan_only_privilege_curri('1', $scope.all_curri_that_have_privileges);
      }
      else {
          $scope.all_curri_that_have_privileges = $rootScope.all_curriculums;
      }
      $http.get('/api/usertype/getexcludeadmcom').success(function (data) {
          $scope.all_usertype = data;
      });
  }
  $scope.$on("modal.show", function (event, args) {
      $scope.init();
  });
   $scope.still_not_complete = function (){
       if(!$scope.new_user_obj){
           return true;
       }
       if(!$scope.new_user_obj.type){
           return true;
       }
       if($rootScope.create_user_ctrl_mode === 1){
           if (!$scope.new_user_obj.new_emails.length)
               return true;
           var index;
           for (index = 0; index < $scope.new_user_obj.new_emails.length; index++) {
               if (!$scope.new_user_obj.new_emails[index]) {
                   return true;
               }
           }
       }
       else{
           return $scope.files.length == 0;
       }
       return false;
   }

  $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
  $scope.add_user_email = function () {
      $scope.new_user_obj.new_emails.push("");
  }
  $scope.remove_email = function (index_to_remove) {
      $scope.new_user_obj.new_emails.splice(index_to_remove, 1);
  }
$scope.save_to_server = function(my_modal) {
    $scope.please_wait = true;
    var formData;
    var configobj;
    if ($rootScope.create_user_ctrl_mode === 1) {
        configobj = {
            method: 'POST',
            url: '/api/users/createnewusersbytyping',
            headers: { 'Content-Type': 'application/json' },
            data: JSON.stringify($scope.new_user_obj)
        };
    }
    else {
        formData = new FormData();
        formData.append("model", angular.toJson($scope.new_user_obj));
        formData.append("file", $scope.files[0]);
        configobj = {
            method: 'POST',
            url: "/api/users/createnewusers",
            headers: { 'Content-Type': undefined },
            data: formData,
            transformRequest: angular.indentity
        };
    }
    $http(configobj).
        success(function (data, status, headers, config) {
            if(!data){
                  my_modal.$hide();
                  alertCaller.success();
            }
            else{
                $rootScope.manage_user_email_duplicate  = data;
                  my_modal.$hide();
              $alert({title:'ดำเนินการสำเร็จบางส่วน', template:'/alert/mycustomtemplate.html',alertType:'warning',duration:10000,
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
        }).
        error(function (data, status, headers, config) {
              $scope.please_wait = false;
              alertCaller.error(null,'บันทึกข้อมูลไม่สำเร็จ เนื่องจาก',data.message);
        });
}

   $scope.$on("fileSelected", function (event, args) {
        $scope.$apply(function () {
            $scope.files = [];
            if(!fileChecker.stdFileChk(args.file)){
                fileChecker.resetFileInput('');
            }
            else{
                $scope.files.push(args.file);
            }   
        });
    });
});
app.controller('manage_user_type_controller', function($scope, $http,$alert,$loading,$rootScope,alertCaller) {
        $scope.please_wait = false;
    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
    });
  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });
    
$scope.init =function() {
$scope.add_these_type= [];
    $scope.please_wait = false;
          $http.get('/api/usertype/getallusertype').success(function (data) {
                $scope.all_usertype = data;
              });
   }

   $scope.still_not_complete = function(){
        if(!$scope.add_these_type){
            return true;
        }
         if($scope.add_these_type.length ==0){
            return true;
        }
        var index;
        for(index=0;index<$scope.add_these_type.length;index++){
            if(!$scope.add_these_type[index]){
                return true;
            }
        }
        return false;
   }
  $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
$scope.add_user_type = function(){   
    $scope.add_these_type.push("");
}
$scope.remove_type = function(index_to_remove){
          $scope.add_these_type.splice(index_to_remove, 1); 
}
  
$scope.save_to_server = function(my_modal) {
  $http.post(
             '/api/usertype/add',
             JSON.stringify($scope.add_these_type),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
                  my_modal.$hide();
                  alertCaller.success();
         })
     .error(function (data, status, headers, config) {
              $scope.please_wait = false;
              alertCaller.error(null,'บันทึกข้อมูลไม่สำเร็จ เนื่องจาก',data.message);
        });
}
});
app.controller('create_survey_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller) {
$scope.init =function() {
                    $scope.my_target = [];
                    $scope.questions =[];
                    $scope.my_survey_name ="";
                $http.get('/api/usertype/getexcludeadmin').success(function (data) {
                $scope.all_usertype = data;
              });
}
  
      $scope.$on("modal.hide", function (event, args) {
     $scope.init();
    });

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });
                    $scope.my_target = [];
                    $scope.questions =[];
                    $scope.my_survey_name ="";

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
              alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  });
    }
});

app.controller('create_edit_research_controller', function($scope, $http,$alert,$loading,$rootScope,fileChecker,alertCaller) {
$scope.init =function() {
    $scope.new_file = [];
    $scope.disabled_search = true;
    $scope.please_wait = false;

    if($rootScope.research_ctrl_mode === 1){
        $scope.mode_txt = "เพิ่ม";
        $scope.research_obj = {};
        $scope.research_obj.curri_id = $rootScope.manage_reseach_my_curri_id_now;
        $scope.research_obj.name = "";
        $scope.research_obj.researcher =[];
        $scope.research_obj.year_publish = "";
    }
    else{
        $scope.disabled_search = false;
        $scope.mode_txt = "แก้ไข";
        $scope.research_obj = $rootScope.manage_research_fix_this_research;
    }
   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
}
    $scope.still_not_complete = function(){
        if(!$scope.research_obj){
            return true;
        }
        if(!$scope.research_obj.name || !$scope.research_obj.year_publish || $scope.research_obj.researcher.length == 0){
            return true;
        }
        if (angular.isNumber($scope.research_obj.year_publish) == false){
            return true;
        }
        if($scope.research_obj.year_publish <=0){
            return true;
        }
        if($scope.disabled_search==true){
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
        my_modal.$hide();
    }
    $scope.set_disabled_search = function(){
        $scope.disabled_search = true;
    }

  $scope.$on("modal.show", function (event, args) {
    $scope.init();
    });

  $scope.save_to_server = function(my_modal){
       $scope.please_wait = true;
       var action,apiurl;
       if($rootScope.research_ctrl_mode === 1){
           action = 'POST';
           apiurl = "/api/research/newresearch";
        }
        else{
            action = 'PUT';
            apiurl = "/api/research/edit";
        }
        var formData = new FormData();
        formData.append("model", angular.toJson($scope.research_obj));
        if($scope.disabled_search == true){
            formData.append("file" , $scope.new_file[0]);
        }
            $http({
                method: action,
                url: apiurl,
                headers: { 'Content-Type': undefined },
                data:formData,
                transformRequest: angular.indentity 
            }).success(function (data, status, headers, config) {
                $rootScope.manage_research_still_same();
                     $rootScope.manage_research_my_research_now = data;
                     $scope.close_modal(my_modal);
                     alertCaller.success();           
            }).
            error(function (data, status, headers, config) {
                $scope.please_wait = false;
                alertCaller.error(null,null,data.message);
            });
    }
        $scope.$on("fileSelected", function (event, args) { 
        $scope.$apply(function () {
            $scope.new_file = [];
            if (!fileChecker.stdFileChk(args.file)) {
                fileChecker.resetFileInput('');
            }
            else {
                $scope.new_file.push(args.file);
            }
        });
    });
});

app.controller('create_edit_lab_controller', function($scope, $http,$alert,$loading,$rootScope,alertCaller) {
$scope.init = function() {
    if($rootScope.lab_ctrl_mode === 1){
        $scope.mode_txt = "เพิ่ม"; 
        $scope.lab_obj = {};
        $scope.lab_obj.name = "";
        $scope.lab_obj.room = "";
        $scope.lab_obj.curri_id = $rootScope.manage_lab_curri_id;
        $scope.lab_obj.aca_year = $rootScope.manage_lab_aca_year;
        $scope.lab_obj.officer = [];
    }
    else{
        $scope.mode_txt = "แก้ไข";
        $scope.lab_obj = $rootScope.manage_lab_fix_this_lab;
    }
}

  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    $scope.still_not_complete = function(){
        if(!$scope.lab_obj){
            return true;
        }
        if(!$scope.lab_obj.name || !$scope.lab_obj.room ){
            return true;
        }
        if( $scope.lab_obj.officer.length == 0){
            return true;
        }     
    }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
     $scope.save_to_server = function(my_modal){
         var action,apiurl;
         if($rootScope.lab_ctrl_mode === 1){
             action = 'POST';
             apiurl = '/api/lablist/newlablist';
         }
         else{
             action = 'PUT';
             apiurl = '/api/lablist/edit';
         }
         $http({
            method: action,
            url: apiurl,
            headers: {'Content-Type': 'application/json'},
            data:JSON.stringify($scope.lab_obj)
        }).success(function (data) {
            $rootScope.manage_lab_still_same();
            $rootScope.manage_lab_my_world_wide_labs = data;
                 $scope.close_modal(my_modal);
            alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  });
    }
});

app.controller('import_to_curri_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller) {
$scope.init =function() {
     $scope.choose_not_complete = true;
              $scope.curri_choosen = {}
         $scope.nothing_change = true;
                $scope.choose_people = [];
             $scope.result= [];
}
 $scope.choose_not_complete = true;
              $scope.curri_choosen = {}
         $scope.nothing_change = true;
                $scope.choose_people = [];

  $scope.close_modal = function(my_modal){
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
              $scope.result_to_del = angular.copy(data);
            $scope.result = [];
              var index;    
              for(index=0;index<$scope.result_to_del.length;index++){
                if($rootScope.all_id_we_have_now_in_curri.indexOf($scope.result_to_del[index].user_id)!=-1 ){           
                    $scope.result_to_del[index].keep = false;
                }else{
                    $scope.result_to_del[index].keep = true;
                }
              }
               for(index=0;index<$scope.result_to_del.length;index++){
                if($scope.result_to_del[index].keep == true){
                     $scope.result.push($scope.result_to_del[index]);
                }
               }
             $scope.choose_not_complete = false;
         });
}
 $scope.save_to_server = function(my_modal){
        $scope.to_sent = {};
        $scope.to_sent.these_people = $scope.choose_people;
        $scope.to_sent.curri_id = $rootScope.manage_bind_curri_id_now;
        
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
                $rootScope.all_id_we_have_now_in_curri = [];
                var index;
    for(index=0;index< $rootScope.manage_bind_all_people_in_curri.length;index++){
          $rootScope.all_id_we_have_now_in_curri.push($rootScope.manage_bind_all_people_in_curri[index].user_id);
    }
                   $scope.close_modal(my_modal);
                   alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  });
    }
});
app.controller('manage_bind_person_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller,$filter) {
$scope.init =function() {
     $scope.choose_not_complete = true;
              $scope.curri_choosen = {};
         $scope.nothing_change = true;
      $scope.all_curri_that_have_privileges = [];
      if($rootScope.current_user.user_type == 'ผู้ดูแลระบบ'){
        $scope.all_curri_that_have_privileges = $rootScope.all_curriculums;
      }else{
         $scope.$parent.scan_only_privilege_curri('2',$scope.all_curri_that_have_privileges);
      }
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
              $scope.curri_choosen = {};
  $scope.nothing_change = true;
   
    $scope.$on("modal.hide", function (event, args) {
     $scope.init();      
    });
  $scope.$on("modal.show", function (event, args) {
        $scope.init();
        var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
        if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.find_information();
    }
    });

    $scope.remove_person = function(index_to_remove,obj){
        if(obj.user_id == $rootScope.current_user.user_id){
            alertCaller.error(null,'ท่านไม่สามารถลบตัวท่านเองออกจากหลักสูตรได้');
        }
        else{
        $rootScope.manage_bind_all_people_in_curri.splice(index_to_remove, 1);    
           $scope.nothing_change = false;
        }
    }
    $scope.find_information = function(){
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
$scope.delete_myself = false;
              $scope.delete_these = [];
        for(index=0;index < $rootScope.manage_bind_all_people_in_curri.length ; index++){
            if($rootScope.manage_bind_all_people_in_curri[index].delete_me == true){
                if($rootScope.manage_bind_all_people_in_curri[index].user_id != $rootScope.current_user.user_id ){
              $scope.delete_these.push($rootScope.manage_bind_all_people_in_curri[index]);
                }
                else{
                    $rootScope.manage_bind_all_people_in_curri[index].delete_me = false;
                    $scope.delete_myself = true;
                    alertCaller.error(null,'ท่านไม่สามารถลบตัวท่านเองออกจากหลักสูตรได้');
                }
            }
        }
        if($scope.delete_these.length != 0){
             for(index =0 ;index<  $scope.delete_these.length ; index++){
            $rootScope.manage_bind_all_people_in_curri.splice($rootScope.manage_bind_all_people_in_curri.indexOf($scope.delete_these[index]),1);
        }
         $scope.nothing_change = false;
        }
        else{
            if( $scope.delete_myself != true){           
                $alert({title:'เกิดข้อผิดพลาด', content:'กรุณาเลือกบุคลากรที่ต้องการนำออก',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
        }
    }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
         $scope.to_sent  = {};
        if($rootScope.manage_bind_all_people_in_curri.length == 0){
            $scope.to_sent.curri_id = $scope.curri_choosen.curri_id;
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
             alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  });
    }
});

app.controller('change_priviledge_by_type_president_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller) {
$scope.init =function() {
     $scope.choose_not_complete = true;
              $scope.curri_choosen = {};
    $scope.not_choose_title_yet = true;
   $scope.privilege_pres_by_type={};
$scope.title_choosen = {};
 $scope.all_curri_that_have_privileges = [];
      $rootScope.curri_that_be_president_in($scope.all_curri_that_have_privileges);

  $http.get('/api/title').success(function (data) {          
             $scope.all_title = data;
           });
}

  $scope.not_choose_title_yet = true;
$scope.title_choosen = {};
  $scope.choose_not_complete = true;
  $scope.curri_choosen = {};
  $scope.privilege_pres_by_type = {};
  
  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    $scope.still_not_complete = function(){
        var index;
        if(!$scope.privilege_pres_by_type){
            return true;
        }
        for(index=0;index<$scope.privilege_pres_by_type.privilege_list.length;index++){
            if(!$scope.privilege_pres_by_type.privilege_list[index].privilege ){
                return true;
            }
        }
           if(angular.equals($scope.copy_save,$scope.privilege_pres_by_type.privilege_list)==true){
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
                   $scope.not_choose_title_yet = false;
              $scope.privilege_pres_by_type = data;
             $scope.choose_not_complete = false;
                     var index;
            var index2;
           
            for(index=0;index< $scope.privilege_pres_by_type.privilege_list.length;index++){
                for(index2=0;index2< $scope.privilege_pres_by_type.choices.length;index2++){
                    if($scope.privilege_pres_by_type.privilege_list[index].privilege.title_privilege_code == $scope.privilege_pres_by_type.choices[index2].title_privilege_code){
                        $scope.privilege_pres_by_type.privilege_list[index].privilege = $scope.privilege_pres_by_type.choices[index2];      
                    }
                }
            }
   $scope.copy_save = angular.copy($scope.privilege_pres_by_type.privilege_list);
         });
    }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
        $http.put(
             '/api/extraprivilegebytype',
             JSON.stringify($scope.privilege_pres_by_type),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.close_modal(my_modal);
             alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  }); 
    }
});

app.controller('education_controller', function($scope, $http,$alert,$loading,$rootScope,AUTH_EVENTS, AuthService,alertCaller) {
$scope.init = function(){
    if($rootScope.edu_ctrl_mode === 1){
        $scope.mode_txt = 'เพิ่ม';
        $scope.edu_obj = {};
        $scope.edu_obj.major = "";
        $scope.edu_obj.college = "";
        $scope.edu_obj.grad_year = "";
        $scope.edu_obj.pre_major = "";
        $scope.edu_obj.degree = "";
    }
    else{
        $scope.mode_txt = 'แก้ไข';
        $scope.edu_obj = $rootScope.manage_profile_fix_this_edu;
    }
}  

$scope.is_not_complete = function(){
    if(!$scope.edu_obj){
        return true;
    }
    if( !$scope.edu_obj.major || !$scope.edu_obj.college || !$scope.edu_obj.grad_year || !$scope.edu_obj.pre_major || !$scope.edu_obj.degree ){
        return true;
    }
    if(isNaN($scope.edu_obj.degree)==true){
        return true;
    }
    if(angular.isNumber($scope.edu_obj.grad_year) == false){
        return true;
    }
    if($scope.edu_obj.grad_year<=0){
        return true;
    }
    return false;
}

  $scope.$on("modal.show", function (event, args) {
      $scope.init();
    });

 $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }

$scope.save_to_server = function(my_modal){
    var action;
    if($rootScope.edu_ctrl_mode === 1){
    $scope.edu_obj.personnel_id = $rootScope.current_user.user_id;
    action = 'POST';
    }
    else{
      action = 'PUT';
    }
            $http({
            method: action,
            url: '/api/education',
            headers: {'Content-Type': 'application/json'},
            data:JSON.stringify($scope.edu_obj)
        }).success(function (data) {
            $rootScope.current_user.information.education = data;
            $rootScope.save_obj.information.education = data;
               $scope.close_modal(my_modal);
               alertCaller.success();
         }).error(function(data, status, headers, config) {
             alertCaller.error(null,null,data.message);
            });
}
});

app.controller('manage_profile_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,AUTH_EVENTS, AuthService,alertCaller) {

$scope.back_to_default = function(){
       angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
       $scope.files= [];
}
$scope.fix_not_complete = function(){
    if(!$rootScope.current_user){ 
        return true;
    }
    if(!$rootScope.current_user.information.e_name || !$rootScope.current_user.information.t_name || !$rootScope.current_user.information.addr || !$rootScope.current_user.information.tel || !$rootScope.current_user.information.email){
        return true;
    }
        if($rootScope.current_user.information.e_prename.length == 14 && $rootScope.current_user.information.e_prename  != 'Assoc.Prof.Dr.'){
               return true;
        }
              if($rootScope.current_user.information.t_prename.length == 14){
               return true;
        }

        if($rootScope.current_user.user_type == 'อาจารย์'){
                  if($rootScope.current_user.information.status.length== 14){
               return true;
        }       
        }
      
    if($rootScope.current_user.information.tel.length <9){
        return true;
    }
    return false;
}

  $scope.$on("modal.show", function (event, args) {
             $scope.init();
    });
       $scope.$on("modal.hide", function (event, args) {
        $rootScope.current_user=  $rootScope.save_obj ;
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
    var index;
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
 $scope.something_change = function(){
    $scope.nothing_change = false;
 }
 $scope.$on("fileSelected", function (event, args) {
        $scope.$apply(function () {            
           var extension = args.file.name.split('.');
 if(args.file.size > 2000000){
                   angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
                $alert({title:'เกิดข้อผิดพลาด', content:'ไฟล์ที่เลือกมีขนาดมากกว่า 2 MB',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
            }
            else if(extension[extension.length-1] == 'exe' || extension[extension.length-1] == 'EXE' || extension[extension.length-1] == 'vb' || extension[extension.length-1] == 'VB'
        || extension[extension.length-1] == 'bat' || extension[extension.length-1] == 'BAT'  || extension[extension.length-1] == 'ini' || extension[extension.length-1] == 'INIT' ){
                     angular.forEach(
    angular.element("input[type='file']"),
    function(inputElem) {
      angular.element(inputElem).val(null);
    });
         $alert({title:'เกิดข้อผิดพลาด', content:'ไม่อนุญาตให้อัพโหลดไฟล์นามสกุลดังกล่าว',alertType:'warning',
                         placement:'bottom-right', effect:'bounce-in',speed:'slow',typeClass:'alertPopFileSize'});
       }
              else if(extension[extension.length-1] != 'jpeg' && extension[extension.length-1]!='jpg' && extension[extension.length-1] != 'png' && extension[extension.length-1] != 'bmp'
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
$scope.go_to_add_edu = function(){
    $rootScope.edu_ctrl_mode = 1;
}
$scope.go_to_fix = function(fix_this_obj){
    $rootScope.manage_profile_fix_this_edu = angular.copy(fix_this_obj);
    $rootScope.edu_ctrl_mode = 2;
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
 alertCaller.success();
        }).
        error(function (data, status, headers, config) {
                      $scope.please_wait = false;
                      alertCaller.error(null,null,data.message);
        });
    }
 });
app.controller('login_controller', function($scope, $http,$alert,$loading,$rootScope,request_all_curriculums_service_server,request_years_from_curri_choosen_service,AUTH_EVENTS, AuthService,alertCaller) {
    $scope.credentials = {
        username: '',
        password: ''
      };
       $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
      $scope.login = function (my_modal) {
        $scope.please_wait = true; 
  $scope.credentials.username.toLowerCase();
        AuthService.login($scope.credentials).then(function (user) {
          $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);  
          $scope.setcurrent_user(user);
             my_modal.$hide();
             alertCaller.success('เข้าสู่ระบบสำเร็จ','ยินดีต้อนรับ '+$rootScope.current_user.username);
               $rootScope.clear_choosen();
                     $scope.please_wait = false;
   if(!!$rootScope.current_user.not_send_primary){
              $rootScope.open_modal_primary_not_send();
         }
        }, function (something) {
            console.log('something')
            console.log(something)
          $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
      $scope.please_wait = false;
      alertCaller.error(null,'เข้าสู่ระบบไม่สำเร็จ ',something.data.message);
        });
      };
});

app.controller('change_priviledge_person_president_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller) {
$scope.init =function() {
     $scope.choose_not_complete = true;
              $scope.curri_choosen = {};
    $scope.not_choose_title_yet = true;
   $scope.privilege_pres_by_person={};
$scope.title_choosen = {};
    $scope.all_curri_that_have_privileges = [];
      $rootScope.curri_that_be_president_in($scope.all_curri_that_have_privileges);

  $http.get('/api/title').success(function (data) {
             $scope.all_title = data;
           });
}

    $scope.not_choose_title_yet = true;
$scope.title_choosen = {};
     $scope.choose_not_complete = true;
              $scope.curri_choosen = {};
  $scope.privilege_pres_by_person = {};
  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });
    $scope.still_not_complete = function(){
        var index;
        if(!$scope.privilege_pres_by_person){
            return true;
        }
        for(index=0;index<$scope.privilege_pres_by_person.privilege_list.length;index++){
            if(!$scope.privilege_pres_by_person.privilege_list[index].privilege ){
                return true;
            }
        }
           if(angular.equals($scope.copy_save,$scope.privilege_pres_by_person.privilege_list)==true){
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
              $scope.privilege_pres_by_person = data;
             $scope.choose_not_complete = false;
               $scope.nothing_change = true;
                     var index;
            var index2;
           
            for(index=0;index< $scope.privilege_pres_by_person.privilege_list.length;index++){
                for(index2=0;index2< $scope.privilege_pres_by_person.choices.length;index2++){      
                    if($scope.privilege_pres_by_person.privilege_list[index].privilege.title_privilege_code == $scope.privilege_pres_by_person.choices[index2].title_privilege_code){
                        $scope.privilege_pres_by_person.privilege_list[index].privilege = $scope.privilege_pres_by_person.choices[index2];
                    }
                }
            }
   $scope.copy_save = angular.copy($scope.privilege_pres_by_person.privilege_list);
         });
    }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
        $http.put(
             '/api/extraprivilege',
             JSON.stringify($scope.privilege_pres_by_person),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.close_modal(my_modal);
             alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  });
    }
});

app.controller('change_priviledge_by_type_admin_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller) {
    $scope.init = function () {
        $scope.not_choose_title_yet = true;
        $scope.privilege_admin_obj = {};
        $scope.title_choosen = {};
        $http.get('/api/title').success(function (data) {
            $scope.all_title = data;
        });
    }

    $scope.not_choose_title_yet = true;
$scope.title_choosen = {};
  $scope.privilege_admin_obj = {};
  
  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    });

    $scope.still_not_complete = function(){
        var index;
        if(!$scope.privilege_admin_obj){
            return true;
        }
        for(index=0;index<$scope.privilege_admin_obj.privilege_list.length;index++){
            if(!$scope.privilege_admin_obj.privilege_list[index].privilege ){
                return true;
            }
        }

        if(angular.equals($scope.copy_save,$scope.privilege_admin_obj.privilege_list)==true){
            return true;
        }
        return false;
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
              $scope.privilege_admin_obj = data;
               var index;
               var index2;
                for(index=0;index< $scope.privilege_admin_obj.privilege_list.length;index++){
                for(index2=0;index2< $scope.privilege_admin_obj.choices.length;index2++){
                    if($scope.privilege_admin_obj.privilege_list[index].privilege.title_privilege_code == $scope.privilege_admin_obj.choices[index2].title_privilege_code){
                        $scope.privilege_admin_obj.privilege_list[index].privilege = $scope.privilege_admin_obj.choices[index2];
                    }
                }
            }
            $scope.copy_save = angular.copy($scope.privilege_admin_obj.privilege_list);
         });
    }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
        $http.put(
             '/api/defaultprivilegebytype',
             JSON.stringify($scope.privilege_admin_obj),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.close_modal(my_modal);
             alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  });
    }
});
app.controller('manage_research_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller,$filter) {
$scope.init =function() {
     $scope.choose_not_complete = true;
              $scope.curri_choosen = {};
         $scope.nothing_change = true;
                $rootScope.manage_research_my_research_now = {};
                 $scope.all_curri_that_have_privileges = [];
  $scope.$parent.scan_only_privilege_curri('9',$scope.all_curri_that_have_privileges);
}

$rootScope.manage_research_still_same = function(){
      $scope.nothing_change = true;
}
     $scope.choose_not_complete = true;
              $scope.curri_choosen = {};
  $scope.nothing_change = true;

      $scope.$on("modal.hide", function (event, args) {
     $scope.init();      
    });
  $scope.$on("modal.show", function (event, args) {
        $scope.init();
        var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
        if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.find_information();
    }
    });
    $scope.download_research = function(path_research){
        $scope.download_file(path_research);
    }
    $scope.download_file = function(path) { 
        window.open(path, '_blank', "");  
    }
    $scope.go_to_fix_research = function(this_research){
        $rootScope.research_ctrl_mode = 2;
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
        $rootScope.manage_research_fix_this_research.researcher = [];   
        var index;
        var inside_index;
        for(index =0;index<this_research.researcher.length;index++){
            for(inside_index=0;inside_index<$rootScope.manage_research_all_teachers_in_curri.length;inside_index++){
                if($rootScope.manage_research_all_teachers_in_curri[inside_index].teacher_id  == this_research.researcher[index].teacher_id){
                          $rootScope.manage_research_fix_this_research.researcher.push($rootScope.manage_research_all_teachers_in_curri[inside_index]);break;
                }
            }
        }
         });
    }   

    $scope.go_to_create_research =function(){
        $rootScope.research_ctrl_mode = 1;
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
    }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
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
             alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  });
    }
});

app.controller('add_committee_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller) {
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
  $scope.close_modal = function(my_modal){
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
        $scope.to_sent = {};
        $scope.to_sent.these_people = $scope.choose_people;
        $scope.to_sent.curri_id = $rootScope.manage_committee_who_curri_id_now;
$scope.to_sent.aca_year = $rootScope.manage_committee_who_aca_year_now;
        
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
                   alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  });
    }
});

app.controller('manage_committee_who_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller,$filter,$q) {
$scope.init =function() {
     $scope.choose_not_complete = true;
              $scope.year_choosen = {};
              $scope.curri_choosen = {};
                   $scope.corresponding_aca_years = [];
         $scope.nothing_change = true;
      $scope.all_curri_that_have_privileges = [];
      $rootScope.curri_that_be_president_in($scope.all_curri_that_have_privileges);
}
     $scope.choose_not_complete = true;
               $scope.year_choosen = {};
              $scope.curri_choosen = {};
  $scope.nothing_change = true;

      $scope.$on("modal.hide", function (event, args) {
     $scope.init();
    });
  $scope.$on("modal.show", function (event, args) {
              $scope.init();
    var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
    if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.sendCurriAndGetYears().then(function(){
            var target_aca_year = $filter('filter')($scope.corresponding_aca_years, {aca_year: $rootScope.current_aca_year},true);
            if(target_aca_year.length > 0){
                $scope.year_choosen = target_aca_year[0];
                $scope.find_information();
            }
        });
    }
    });

      $scope.sendCurriAndGetYears = function () {
        $scope.choose_not_complete =true;
        $scope.year_choosen = {}
         $scope.nothing_change = true;
    
        var promise = request_years_from_curri_choosen_service.async($scope.curri_choosen,999).then(function(data) {
            $scope.corresponding_aca_years = data;
            return $q.resolve("");
          });
          return promise;
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
        my_modal.$hide();
    }
    $scope.save_to_server = function(my_modal){
        $scope.to_sent = {};   
           $scope.to_sent.aca_year = $scope.year_choosen.aca_year ;
            $scope.to_sent.curri_id = $scope.curri_choosen.curri_id ;
            $scope.to_sent.these_people = $rootScope.manage_committee_who_all_committees;
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
             alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  });
    }
});

app.controller('show_education_personnel_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,Lightbox,$filter) {
$scope.init =function() {
     $scope.choose_not_complete = true;
              $scope.curri_choosen = {};
                 $scope.all_curri_that_have_privileges = [];
                 $scope.result = {};
  $scope.$parent.scan_only_privilege_curri('7',$scope.all_curri_that_have_privileges);
}
    $scope.$on("modal.hide", function (event, args) {
     $scope.init();
    });
  $scope.$on("modal.show", function (event, args) {
        $scope.init();
        var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
        if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.find_information();
    }
    });
$scope.init();
   $scope.openLightboxModal = function (pic) {
    $scope.fake_array = [];
    $scope.fake_array.push(pic.file_name_pic);
    Lightbox.openModal($scope.fake_array, 0);
  };

    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    };

    $scope.find_information = function(){
        $http.post(
             '/api/personnel/getwitheducation',
             JSON.stringify($scope.curri_choosen.curri_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.result =data;
             $scope.choose_not_complete = false;
         });
}
});

app.controller('change_password_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller) {
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
        $scope.to_sent = {};
        $scope.to_sent.user_id =   $rootScope.current_user.user_id;
         $scope.to_sent.old_password = $scope.old_password;
        $scope.to_sent.new_password = $scope.new_password;
      
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
                  alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,'รหัสผ่านเก่าไม่ถูกต้อง');
  });
  }
});

app.controller('change_username_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller) {
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
        $scope.to_sent = {};
        $scope.to_sent.username = $scope.new_username;
        $scope.to_sent.user_id =   $rootScope.current_user.user_id; 
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
              $rootScope.save_obj.username = $scope.to_sent.username;
                  $scope.close_modal(my_modal);
                  alertCaller.success();   
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,'ชื่อผู้ใช้นี้มีอยู่แล้วในระบบ');
  });
  }
});

app.controller('create_album_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,fileChecker,alertCaller) {
$scope.my_pictures = {};
$scope.init =function() {
   $scope.please_wait = false;
        $scope.my_new_album = {};
      $scope.my_new_album.name = "";
$scope.my_pictures.flow.cancel();
}
$scope.please_wait = false;
$scope.my_new_album = {};
$scope.my_new_album.name = "";
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
        my_modal.$hide();
    }
$scope.imgFileCheck = function(file){
    return fileChecker.imgFileChk(file);
}
    $scope.save_to_server = function(my_modal) {
           $scope.please_wait = true;
 $scope.my_new_album.curri_id =    $rootScope.manage_album_curri_id;
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
            formData.append("picture"+(index+1), $scope.my_pictures.flow.files[index].file);
        }
        formData.append("model", angular.toJson( $scope.my_new_album));
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
                alertCaller.success();   
        }).
        error(function (data, status, headers, config) {
              $scope.please_wait = false;
              alertCaller.error(null,null,data.message);
        });
    }
});

app.controller('create_edit_minute_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,Lightbox,fileChecker,alertCaller) {
$scope.my_pictures = {}; /*Pre initial for flow object*/
$scope.init =function() {
    $scope.please_wait = false;
    $scope.disabled_search = true;
    $scope.minute_obj = {};
    $scope.minute_obj.date = "";
    $scope.minute_obj.attendee = [];
    $scope.minute_obj.topic_name = "";
    $scope.my_file = [];
    $scope.old_pictures = [];
    $scope.my_pictures.flow.cancel();
    angular.forEach(
        angular.element("input[type='file']"),
        function (inputElem) {
            angular.element(inputElem).val(null);
        });
    if ($rootScope.minutes_ctrl_mode === 1) {
        $scope.mode_txt = "เพิ่ม";
        $scope.add_more_pic_txt = "";
        $scope.minute_obj.curri_id = $rootScope.manage_minutes_curri_id;
        $scope.minute_obj.aca_year = $rootScope.manage_minutes_aca_year;
        $scope.minute_obj.teacher_id = $rootScope.current_user.user_id;
    }
    else {
        $scope.mode_txt = "แก้ไข";
        $scope.add_more_pic_txt = "เพิ่ม";
        $scope.disabled_search = false;
        $scope.minute_obj = $rootScope.manage_minutes_fix_this_minute;
        var index;
        for(index = 0;index < $scope.minute_obj.pictures.length;index++){
            $scope.old_pictures.push($scope.minute_obj.pictures[index]);
        }
    }
        $scope.minute_obj.pictures = [];
}
  $scope.$on("modal.show", function (event, args) {
      $scope.init();
    });
    $scope.$on("fileSelected", function (event, args) {
        $scope.$apply(function () {
            $scope.my_file = [];
            if (!fileChecker.stdFileChk(args.file)) {
                fileChecker.resetFileInput('');
            }
            else {
                $scope.my_file.push(args.file);
            }
        });
    });
$scope.imgFileCheck = function(file){
    return fileChecker.imgFileChk(file);
}
     $scope.return_just_name = function(full_name){
        var set = full_name.split('/');
        return set[set.length-1]
     }
    $scope.set_disabled_search = function(){  
        $scope.disabled_search = true;
    }
    $scope.watch_file = function(){
        window.open($scope.minute_obj.file_name, '_blank', "width=800, left=230,top=0,height=700");
    }
     $scope.still_not_complete = function(){
        if(!$scope.my_pictures.flow){
            return true;
        }
        if(!$scope.minute_obj){
            return true;
        }
        if(!$scope.minute_obj.topic_name || !$scope.minute_obj.date || $scope.minute_obj.attendee.length ==0){
            return true;
        }
        else{
            if($scope.disabled_search == true){
                if($scope.my_file.length ==0){
                    return true;
                }       
            }
        }
        return false;    
     }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
    $scope.delete_picture = function(index_pic){
        $scope.old_pictures.splice(index_pic,1);
    }
    $scope.save_to_server = function(my_modal) {
   $scope.please_wait = true;
      var formData = new FormData();
        var index,configobj;
        //Old minute picture obj
        for (index = 0; index < $scope.old_pictures.length; index++) {
            $scope.minute_obj.pictures.push($scope.old_pictures[index]);
        }
        //New minute picture obj
        for (index = 0 ;index< $scope.my_pictures.flow.files.length;index++){
        $scope.my_obj = {};
        $scope.my_obj.minutes_id = 0;
        $scope.my_obj.file_name = $scope.my_pictures.flow.files[index].file.name;
            $scope.minute_obj.pictures.push($scope.my_obj);
            formData.append("picture"+(index+1), $scope.my_pictures.flow.files[index].file);
        }
   if($scope.disabled_search == true){
      $scope.minute_obj.file_name = $scope.my_file[0].name;
      formData.append("file", $scope.my_file[0]);
    }
    else{
          $scope.minute_obj.file_name = "";
    }
       formData.append("model", angular.toJson($scope.minute_obj));
       if ($rootScope.minutes_ctrl_mode === 1) {
           configobj = {
               method: 'POST',
               url: "/api/minutes/add",
               headers: { 'Content-Type': undefined },
               data: formData,
               transformRequest: angular.indentity
           };
       }
       else {
           configobj = {
               method: 'PUT',
               url: "/api/minutes/edit",
               headers: { 'Content-Type': undefined },
               data: formData,
               transformRequest: angular.indentity
           };
       }
        $http(configobj).
        success(function (data, status, headers, config) {
        $rootScope.manage_minutes_still_same();
                $rootScope.manage_minutes_my_world_wide_minutes =data;
                      $rootScope.manage_minutes_my_world_wide_minutes_fix_year = angular.copy($rootScope.manage_minutes_my_world_wide_minutes );
         var index;
         for(index=0;index<$rootScope.manage_minutes_my_world_wide_minutes_fix_year.length;index++){
          $rootScope.manage_minutes_my_world_wide_minutes_fix_year[index].date = $rootScope.manage_minutes_my_world_wide_minutes_fix_year[index].date.split("/")[0]  + "/" + $rootScope.manage_minutes_my_world_wide_minutes_fix_year[index].date.split("/")[1]+ "/" +( parseInt($rootScope.manage_minutes_my_world_wide_minutes_fix_year[index].date.split("/")[2])+543);
         }
                $scope.close_modal(my_modal);
                alertCaller.success();        
        }).
        error(function (data, status, headers, config) {
              $scope.please_wait = false;
              alertCaller.error(null,null,data.message);
        });
    }
 });
app.controller('manage_minutes_show_images_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,Lightbox) {
 $scope.openLightboxModal = function (index) {
    Lightbox.openModal($rootScope.manage_minutes_show_images_of_this_minute.pictures, index);
  };
});
app.controller('manage_minutes_controller', function($scope, $http,$alert,$loading,request_all_curriculums_service_server,$rootScope,request_years_from_curri_choosen_service,alertCaller,$filter,$q) {
$scope.init =function() {
     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {};
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
        var target_curri = $filter('filter')($scope.all_curri_that_have_privileges, {curri_id: $rootScope.current_curriculum},true);
    if(target_curri.length > 0){
        $scope.curri_choosen = target_curri[0];
        $scope.sendCurriAndGetYears().then(function(){
            var target_aca_year = $filter('filter')($scope.corresponding_aca_years, {aca_year: $rootScope.current_aca_year},true);
            if(target_aca_year.length > 0){
                $scope.year_choosen = target_aca_year[0];
                $scope.find_information();
            }
        });
    }
    });

     $scope.choose_not_complete = true;
         $scope.year_choosen = {};
              $scope.curri_choosen = {};
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
              var promise = request_years_from_curri_choosen_service.async($scope.curri_choosen,8,2).then(function(data) {
 $scope.year_choosen = {};
            $scope.corresponding_aca_years = data;
            return $q.resolve("");
          });
          return promise;
    }
    $scope.go_to_fix_minute = function(this_minute_index){
        $rootScope.minutes_ctrl_mode = 2;
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
        $rootScope.manage_minutes_fix_this_minute = angular.copy($rootScope.manage_minutes_my_world_wide_minutes[this_minute_index]);
        var index;
        var inside_index;
        $rootScope.manage_minutes_fix_this_minute.attendee = [];
        for(index =0;index<$rootScope.manage_minutes_my_world_wide_minutes[this_minute_index].attendee.length;index++){
            for(inside_index=0;inside_index<$rootScope.manage_minutes_all_teachers_in_curri.length;inside_index++){
                if($rootScope.manage_minutes_all_teachers_in_curri[inside_index].teacher_id == $rootScope.manage_minutes_my_world_wide_minutes[this_minute_index].attendee[index].teacher_id){
                          $rootScope.manage_minutes_fix_this_minute.attendee.push($rootScope.manage_minutes_all_teachers_in_curri[inside_index]);
                }
            }
        }
});  
    }
    $scope.go_to_see_attendee = function(this_minute){
        $rootScope.manage_minutes_see_this_attendee = this_minute.attendee;
    }
    $scope.go_to_create_minute =function(){
        $rootScope.minutes_ctrl_mode = 1;
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
         $rootScope.manage_minutes_my_world_wide_minutes_fix_year.splice(index_to_remove, 1);   
    }
    $scope.find_information = function(){
        $http.post(
             '/api/minutes/getminutes',
             JSON.stringify($scope.year_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.nothing_change = true;
               $rootScope.manage_minutes_my_world_wide_minutes = data;
         $rootScope.manage_minutes_my_world_wide_minutes_fix_year = angular.copy( $rootScope.manage_minutes_my_world_wide_minutes );
         var index;
         for(index=0;index<$rootScope.manage_minutes_my_world_wide_minutes_fix_year.length;index++){      
          $rootScope.manage_minutes_my_world_wide_minutes_fix_year[index].date = $rootScope.manage_minutes_my_world_wide_minutes_fix_year[index].date.split("/")[0]  + "/" + $rootScope.manage_minutes_my_world_wide_minutes_fix_year[index].date.split("/")[1]+ "/" +( parseInt($rootScope.manage_minutes_my_world_wide_minutes_fix_year[index].date.split("/")[2])+543);
         }
             $scope.choose_not_complete = false;
         });
    }
    $scope.close_modal = function(my_modal){
        my_modal.$hide();
    }
    $scope.download_file = function(path) { 
        window.open(path, '_blank', "");  
    }
    $scope.save_to_server = function(my_modal){
        if($rootScope.manage_minutes_my_world_wide_minutes_fix_year.length == 0){
            $scope.to_sent  = {};
            $scope.to_sent.curri_id  = $scope.curri_choosen.curri_id;
            $scope.to_sent.aca_year = $scope.year_choosen.aca_year;
            $rootScope.manage_minutes_my_world_wide_minutes_fix_year.push($scope.to_sent);
        }
        $http.put(
             '/api/minutes/delete',
             JSON.stringify($rootScope.manage_minutes_my_world_wide_minutes_fix_year),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
            $scope.close_modal(my_modal);
            alertCaller.success();
         })
    .error(function(data, status, headers, config) {
        alertCaller.error(null,null,data.message);
  });
    }
});

app.service('manage_users_data',function($http,$q,$rootScope,alertCaller){
    var serviceObj = new Object();
    serviceObj.user_list_data = {};
    serviceObj.set_user_list_data = function(value){
        serviceObj.user_list_data = value;
    };
    serviceObj.update_user_list_data = function(value){
        if(angular.isUndefined(value)){
            var target_curri_id;
            if($rootScope.is_admin())
                target_curri_id = "0";
            else{
                target_curri_id = Object.keys($rootScope.current_user.president_in)[0];
            }
            $http.post(
            '/api/users/getuserlist',
            JSON.stringify(target_curri_id),
            {
                headers: {
                    'Content-Type': 'application/json'
                }
            }
            ).
            
            then(function (response) {
                serviceObj.user_list_data.user_list = response.data;
            }, function (error) {
                alertCaller.error(null, 'ไม่สามารถโหลดรายชื่อผู้ใช้งานได้ กรุณาลองใหม่อีกครั้ง');
            });
        }
        else{
            serviceObj.user_list_data.user_list = value;
        }
    };

    serviceObj.set_user_id_for_edit = function(value){
        serviceObj.user_id_for_edit = value;
    };
    serviceObj.get_user_for_edit = function(){
        var promise_obj = $http.post(
            '/api/users/getuserdataforedit',
            JSON.stringify(serviceObj.user_id_for_edit),
            {
                headers: {
                    'Content-Type': 'application/json'
                }
            }
        ).then(function(response){
            return response.data;
        },function(error){
            return $q.reject(error.data);
        });
        return promise_obj;
    }
    return serviceObj;
});


app.controller('manage_users_data_main_controller', function($scope, $http,$alert,request_all_curriculums_service_server,$rootScope,alertCaller,manage_users_data){
$scope.$on('modal.show',function(event,args){
    $scope.main_obj = {};
    $scope.main_obj.user_list = [];
    manage_users_data.set_user_list_data($scope.main_obj);
    manage_users_data.update_user_list_data();
});

$scope.go_to_edit = function(index_to_edit){
    manage_users_data.set_user_id_for_edit($scope.main_obj.user_list[index_to_edit].user_id);
};
});

app.controller('edit_user_data_direct_controller', function($scope, $http,$alert,$rootScope,$modalBox,alertCaller,manage_users_data){
    $scope.edit_profile_obj = {};
    $scope.e_prename_choices = ['Mr.','Mrs.','Miss.','Dr.','Asst.Prof.Dr.','Asst.Prof.','Assoc.Prof.Dr.','Assoc.Prof.','Prof.','Prof.Dr.'];
    $scope.t_prename_choices = ['นาย','นางสาว','นาง','ดร.','ผศ.ดร.','รศ.ดร.','ศ.ดร.','ผศ.','รศ.','ศ.'];

$scope.$on('modal.show',function(event,args){
    $scope.edit_profile_obj = {};
    manage_users_data.get_user_for_edit().then(function(data){
        $scope.edit_profile_obj = data;
    },function(error){
        alertCaller.error(null, error.exceptionMessage);
        args.hide();
    });
    $scope.please_wait = false;
});
$scope.close_modal = function(my_modal){
    my_modal.$hide();
}
$scope.not_complete = function(){
    if(angular.isUndefined($scope.edit_profile_obj.main_info))
    return true;
    if(!$scope.edit_profile_obj.main_info.email)
    return true;
    else
    return false;
};

 $scope.show_confirm_resetpwd = function (username) {
                var boxOptions = {
                content: 'คุณต้องการ reset รหัสผ่านของผู้ใช้งาน '+username+' ใช่หรือไม่?<br>(รหัสผ่านจะถูก reset เป็น 1234)',
                title:'แจ้งเตือน',
                theme:'danger',
                boxType: 'confirm',
                backdrop:'static',
                confirmText:'ใช่',
                cancelText:'ไม่',
                effect:'bounce-in',
                afterConfirm:function(){ $scope.reset_password();},
                }
                $modalBox(boxOptions);
            }

$scope.reset_password = function(){
    $http.post(
            '/api/users/resetpwd',
            JSON.stringify($scope.edit_profile_obj.user_id),
            {
                headers: {
                    'Content-Type': 'application/json'
                }
            }
        ).then(function(response){
            alertCaller.success(null,"reset รหัสผ่านแล้ว");
        },function(error){
            if(error.status == 400)
            alertCaller.error(null,error.data.message);
            else
            alertCaller.error();
        });
}


$scope.save_to_server = function(my_modal){
    $scope.please_wait = true;
    var formData = new FormData();
    formData.append("model", angular.toJson($scope.edit_profile_obj));
    //formData.append("file" , $scope.files[0]);
    $http({
            method: 'PUT',
            url: "/api/users/edituserdatadirect",
            headers: { 'Content-Type': undefined },
            data:formData,
            transformRequest: angular.indentity 
        }).then(function(response){
            manage_users_data.update_user_list_data();
            alertCaller.success();
            my_modal.$hide();
        },function(error){
            if(error.status == 400)
            alertCaller.error(null,error.data.message);
            else
            alertCaller.error();
            $scope.please_wait = false;
        });
};
});

app.controller('select_curriculum_aca_year_controller', function($scope, $http,$alert,$rootScope,alertCaller,$filter){
    $scope.curri_choosen = "";
    $scope.aca_year_choosen = "";
    $scope.aca_year_choices = [];
    $scope.indicator_choosen = "";
    $scope.indicator_choices = [];
$scope.$on('modal.show',function(event,args){
    $scope.curri_choosen = "";
    $scope.aca_year_choosen = "";
    $scope.aca_year_choices = [];
    $scope.indicator_choosen = "";
    $scope.indicator_choices = [];
});

$scope.showcurrentcurriaca = function(){
    if(!$rootScope.current_aca_year)
    return "ยังไม่มีการเลือก";
    var target = $filter('filter')($rootScope.all_curriculums, {curri_id: $rootScope.current_curriculum},true);
    return target[0].curr_tname + " ปีการศึกษา " + $rootScope.current_aca_year;
};

$scope.showcurrentindicator = function(){
    if(!$rootScope.current_indicator)
    return "ยังไม่มีการเลือกตัวบ่งชี้";
    else
    return "AUN " +$rootScope.current_indicator.indicator_num+ ' ' + $rootScope.current_indicator.indicator_name_e;
};

$scope.choose_not_complete = function(){
    if(!$scope.curri_choosen)
    return true;
    else if(!$scope.aca_year_choosen)
    return true;
    else
    return false;
};

$scope.get_aca_year = function(){
    $scope.aca_year_choices = [];
    $scope.aca_year_choosen = "";
    $scope.indicator_choices = [];
    $scope.indicator_choosen = "";
    $http.post(
             '/api/curriculumacademic/getbycurriculum',
             JSON.stringify($scope.curri_choosen),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.aca_year_choices = data;
         })
         .error(function(data, status, headers, config) {
             alertCaller.error(null,'กรุณาเลือกหลักสูตรใหม่อีกครั้ง');
  });
};

$scope.get_indicators = function(){
    $scope.indicator_choices = [];
    $scope.indicator_choosen = "";
    $http.post(
        '/api/indicator/querybycurriculumacademic',
        JSON.stringify($scope.aca_year_choosen),
        {
            headers: {
                'Content-Type': 'application/json'
            }
        }).success(function (data) {
             $scope.indicator_choices = data;
         }).error(function(data, status, headers, config) {
             alertCaller.error(null,'กรุณาเลือกปีการศึกษาใหม่อีกครั้ง');
            });
};

$scope.confirmselect = function(my_modal){
    $rootScope.current_curriculum = $scope.curri_choosen.curri_id;
    $rootScope.current_aca_year = $scope.aca_year_choosen.aca_year;
    $rootScope.current_indicator = $scope.indicator_choosen;
    alertCaller.success(null,"เลือกหลักสูตร-ปีการศึกษาเรียบร้อย");
    my_modal.$hide();
};
});









