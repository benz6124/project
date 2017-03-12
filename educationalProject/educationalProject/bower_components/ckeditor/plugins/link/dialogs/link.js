// *
//  * @license Copyright (c) 2003-2016, CKSource - Frederico Knabben. All rights reserved.
//  * For licensing, see LICENSE.md or http://ckeditor.com/license
//  */

'use strict';

( function() {

	var items_evidence =window.big_chunk;
							
	CKEDITOR.dialog.add( 'link', function( editor ) {
		var plugin = CKEDITOR.plugins.link;

		// Handles the event when the "Target" selection box is changed.
		var targetChanged = function() {
				var dialog = this.getDialog(),
					popupFeatures = dialog.getContentElement( 'target', 'popupFeatures' ),
					targetName = dialog.getContentElement( 'target', 'linkTargetName' ),
					value = this.getValue();

				if ( !popupFeatures || !targetName )
					return;

				popupFeatures = popupFeatures.getElement();
				popupFeatures.hide();
				targetName.setValue( '' );

				switch ( value ) {
					case 'frame':
						targetName.setLabel( editor.lang.link.targetFrameName );
						targetName.getElement().show();
						break;
					case 'popup':
						popupFeatures.show();
						targetName.setLabel( editor.lang.link.targetPopupName );
						targetName.getElement().show();
						break;
					default:
						targetName.setValue( value );
						targetName.getElement().hide();
						break;
				}

			};

		// Handles the event when the "Type" selection box is changed.
		var linkTypeChanged = function() {
				var dialog = this.getDialog(),
					partIds = [ 'urlOptions', 'anchorOptions', 'emailOptions','evidencesOptions' ],
					typeValue = this.getValue(),
					uploadTab = dialog.definition.getContents( 'upload' ),
					uploadInitiallyHidden = uploadTab && uploadTab.hidden;

				if ( typeValue == 'url' || typeValue == 'evidences' ) {
					if ( editor.config.linkShowTargetTab )
						dialog.showPage( 'target' );
					if ( !uploadInitiallyHidden )
						dialog.showPage( 'upload' );
				} else {
					dialog.hidePage( 'target' );
					if ( !uploadInitiallyHidden )
						dialog.hidePage( 'upload' );
				}

				for ( var i = 0; i < partIds.length; i++ ) {
					var element = dialog.getContentElement( 'info', partIds[ i ] );
					if ( !element )
						continue;

					element = element.getElement().getParent().getParent();
					if ( partIds[ i ] == typeValue + 'Options' )
						element.show();
					else
						element.hide();
				}

				dialog.layout();
			};

		var setupParams = function( page, data ) {
				if ( data[ page ] )
					this.setValue( data[ page ][ this.id ] || '' );
			};

		var setupPopupParams = function( data ) {
				return setupParams.call( this, 'target', data );
			};

		var setupAdvParams = function( data ) {
				return setupParams.call( this, 'advanced', data );
			};

		var commitParams = function( page, data ) {
				if ( !data[ page ] )
					data[ page ] = {};

				data[ page ][ this.id ] = this.getValue() || '';
			};

		var commitPopupParams = function( data ) {
				return commitParams.call( this, 'target', data );
			};

		var commitAdvParams = function( data ) {
				return commitParams.call( this, 'advanced', data );
			};

		var commonLang = editor.lang.common,
			linkLang = editor.lang.link,
			anchors;

		return {
			title: linkLang.title,
			minWidth: 350,
			minHeight: 230,
			contents: [ {
				id: 'info',
				label: linkLang.info,
				title: linkLang.info,
				elements: [ {
					id: 'linkType',
					type: 'select',
					label: linkLang.type,
					'default': 'url',
					items: [
						[ linkLang.toUrl, 'url' ],
						[ linkLang.toAnchor, 'anchor' ],
						[ linkLang.toEmail, 'email' ],
									[ 'หลักฐานสนับสนุน', 'evidences' ]
					],
					onChange: linkTypeChanged,
					setup: function( data ) {
						this.setValue( data.type || 'url' );
					},
					commit: function( data ) {
						data.type = this.getValue();
					}
				},
				{
					type: 'vbox',
					id: 'urlOptions',
					children: [ {
						type: 'hbox',
						widths: [ '25%', '75%' ],
						children: [ {
							id: 'protocol',
							type: 'select',
							label: commonLang.protocol,
							'default': 'http://',
							items: [
								// Force 'ltr' for protocol names in BIDI. (#5433)
								[ 'http://\u200E', 'http://' ],
								[ 'https://\u200E', 'https://' ],
								[ 'ftp://\u200E', 'ftp://' ],
								[ 'news://\u200E', 'news://' ],
								[ linkLang.other, '' ]
							],
							setup: function( data ) {
								if ( data.url )
									this.setValue( data.url.protocol || '' );
							},

							commit: function( data ) {
								if ( !data.url )
									data.url = {};

								data.url.protocol = this.getValue();
							}
						},
						{
							type: 'text',
							id: 'url',
							label: commonLang.url,
							required: true,
							onLoad: function() {
								this.allowOnChange = true;
							},
							onKeyUp: function() {
								this.allowOnChange = false;
								var protocolCmb = this.getDialog().getContentElement( 'info', 'protocol' ),
									url = this.getValue(),
									urlOnChangeProtocol = /^(http|https|ftp|news):\/\/(?=.)/i,
									urlOnChangeTestOther = /^((javascript:)|[#\/\.\?])/i;

								var protocol = urlOnChangeProtocol.exec( url );
								if ( protocol ) {
									this.setValue( url.substr( protocol[ 0 ].length ) );
									protocolCmb.setValue( protocol[ 0 ].toLowerCase() );
								} else if ( urlOnChangeTestOther.test( url ) ) {
									protocolCmb.setValue( '' );
								}

								this.allowOnChange = true;
							},
							onChange: function() {
								if ( this.allowOnChange ) // Dont't call on dialog load.
								this.onKeyUp();
							},
							validate: function() {
								var dialog = this.getDialog();

								if ( dialog.getContentElement( 'info', 'linkType' ) && dialog.getValueOf( 'info', 'linkType' ) != 'url' )
									return true;

								if ( !editor.config.linkJavaScriptLinksAllowed && ( /javascript\:/ ).test( this.getValue() ) ) {
									alert( commonLang.invalidValue ); // jshint ignore:line
									return false;
								}

								if ( this.getDialog().fakeObj ) // Edit Anchor.
								return true;

								var func = CKEDITOR.dialog.validate.notEmpty( linkLang.noUrl );
								return func.apply( this );
							},
							setup: function( data ) {
								this.allowOnChange = false;
								if ( data.url )
									this.setValue( data.url.url );
								this.allowOnChange = true;

							},
							commit: function( data ) {
								// IE will not trigger the onChange event if the mouse has been used
								// to carry all the operations #4724
								this.onChange();

								if ( !data.url )
									data.url = {};

								data.url.url = this.getValue();
								this.allowOnChange = false;
							}
						} ],
						setup: function() {
							if ( !this.getDialog().getContentElement( 'info', 'linkType' ) )
								this.getElement().show();
						}
					},
 ]
				},{
					type: 'vbox',
					id: 'evidencesOptions',
					children: [ {
						type: 'hbox',
						widths: [ '25%', '75%' ],
						children: [ 
							{
								id: 'choice_curri',
								type: 'select',
								label: 'กรุณาเลือกหลักฐาน',
								// 'default': 'http://',
								items: 
								[]
								,
								setup: function( data ) {
									var evidenceselectopt = this;
									var httpservice = angular.injector(["ng"]).get("$http");
									httpservice.post(
										'/api/evidence/getallevidence',
										JSON.stringify(window.currentcurrisel),
										{
											headers: {
												'Content-Type': 'application/json'
											}
										}
										).then(function (data) {
											window.big_chunk = data.data;
											evidenceselectopt.clear();
											var index;
											var evidencearray = window.big_chunk;
											for(index = 0;index < evidencearray.length;index++){
												evidenceselectopt.add(evidencearray[index][0], evidencearray[index][1]);
											}
										},function(error){
											console.log("Error on loading evidence data. Use old evidence data instead.");
										});
						},
								commit: function( data ) {
									if ( !data.evidences )
										data.evidences = {};
													console.log('this')
									console.log(this)
									console.log(this.getElement())
									console.log('-------')
									data.evidences.path = this.getValue().split(',')[0];
									data.evidences.code = this.getValue().split(',')[1];
									console.log
								}
						// 		,
						// onChange: function(){
						// 		curri_now = this.getValue();
						// 			console.log(curri_now)

						// 		items_year_na = 		[	[ '525200E' ],
						// 		[ '2525200E' ],
						// 		[ '2500E'],
						// 		[ '257200E' ]];
						// 		console.log('items_year_na')
						// 		console.log(items_year_na)

						// 	}
							},

// 							{
// 								id: 'choice_year',
// 								type: 'select',
// 								label: 'กรุณาเลือกปีการศึกษา',
// 								// 'default': 'http://',
// 								items: items_year
// 								,
// 								setup: function( data ) {
					
// console.log('setup')
// 							var linkType = this.getDialog().getContentElement( 'info', 'linkType' );
// 							if ( linkType && linkType.getValue() == 'evidences' ){
// 								this.focus();
								
								
// 							}
// 						},
// 								commit: function( data ) {
// 									if ( !data.evidences )
// 										data.evidences = {};

// 									data.evidences.year = this.getValue();
// 								}		,

// 						onChange: function(){
// 								year_now = this.getValue();

// 							},
// 							onFocus:function(){
// 								console.log('onfocus')
// items_year = items_year_na;
// 	console.log(items_year)
// 							}
// 							},

						 ],
						setup: function() {

						if ( !this.getDialog().getContentElement( 'info', 'linkType' ) )
							this.getElement().hide();



						}
					}  ]
				},
				{
					type: 'vbox',
					id: 'anchorOptions',
					width: 260,
					align: 'center',
					padding: 0,
					children: [ {
						type: 'fieldset',
						id: 'selectAnchorText',
						label: linkLang.selectAnchor,
						setup: function() {
							anchors = plugin.getEditorAnchors( editor );

							this.getElement()[ anchors && anchors.length ? 'show' : 'hide' ]();
						},
						children: [ {
							type: 'hbox',
							id: 'selectAnchor',
							children: [ {
								type: 'select',
								id: 'anchorName',
								'default': '',
								label: linkLang.anchorName,
								style: 'width: 100%;',
								items: [
									[ '' ]
								],
								setup: function( data ) {
									this.clear();
									this.add( '' );

									if ( anchors ) {
										for ( var i = 0; i < anchors.length; i++ ) {
											if ( anchors[ i ].name )
												this.add( anchors[ i ].name );
										}
									}

									if ( data.anchor )
										this.setValue( data.anchor.name );

									var linkType = this.getDialog().getContentElement( 'info', 'linkType' );
									if ( linkType && linkType.getValue() == 'email' )
										this.focus();
								},
								commit: function( data ) {
									if ( !data.anchor )
										data.anchor = {};

									data.anchor.name = this.getValue();
								}
							},
							{
								type: 'select',
								id: 'anchorId',
								'default': '',
								label: linkLang.anchorId,
								style: 'width: 100%;',
								items: [
									[ '' ]
								],
								setup: function( data ) {
									this.clear();
									this.add( '' );

									if ( anchors ) {
										for ( var i = 0; i < anchors.length; i++ ) {
											if ( anchors[ i ].id )
												this.add( anchors[ i ].id );
										}
									}

									if ( data.anchor )
										this.setValue( data.anchor.id );
								},
								commit: function( data ) {
									if ( !data.anchor )
										data.anchor = {};

									data.anchor.id = this.getValue();
								}
							} ],
							setup: function() {
								this.getElement()[ anchors && anchors.length ? 'show' : 'hide' ]();
							}
						} ]
					},
					{
						type: 'html',
						id: 'noAnchors',
						style: 'text-align: center;',
						html: '<div role="note" tabIndex="-1">' + CKEDITOR.tools.htmlEncode( linkLang.noAnchors ) + '</div>',
						// Focus the first element defined in above html.
						focus: true,
						setup: function() {
							this.getElement()[ anchors && anchors.length ? 'hide' : 'show' ]();
						}
					} ],
					setup: function() {
						if ( !this.getDialog().getContentElement( 'info', 'linkType' ) )
							this.getElement().hide();
					}
				},
				{
					type: 'vbox',
					id: 'emailOptions',
					padding: 1,
					children: [ {
						type: 'text',
						id: 'emailAddress',
						label: linkLang.emailAddress,
						required: true,
						validate: function() {
							var dialog = this.getDialog();

							if ( !dialog.getContentElement( 'info', 'linkType' ) || dialog.getValueOf( 'info', 'linkType' ) != 'email' )
								return true;

							var func = CKEDITOR.dialog.validate.notEmpty( linkLang.noEmail );
							return func.apply( this );
						},
						setup: function( data ) {
							if ( data.email )
								this.setValue( data.email.address );

							var linkType = this.getDialog().getContentElement( 'info', 'linkType' );
							if ( linkType && linkType.getValue() == 'email' )

								this.select();
						},
						commit: function( data ) {
							if ( !data.email )
								data.email = {};

							data.email.address = this.getValue();
						}
					},
					{
						type: 'text',
						id: 'emailSubject',
						label: linkLang.emailSubject,
						setup: function( data ) {
							if ( data.email )
								this.setValue( data.email.subject );
						},
						commit: function( data ) {
							if ( !data.email )
								data.email = {};

							data.email.subject = this.getValue();
						}
					},
					{
						type: 'textarea',
						id: 'emailBody',
						label: linkLang.emailBody,
						rows: 3,
						'default': '',
						setup: function( data ) {
							if ( data.email )
								this.setValue( data.email.body );
						},
						commit: function( data ) {
							if ( !data.email )
								data.email = {};

							data.email.body = this.getValue();
						}
					} ],
					setup: function() {
						if ( !this.getDialog().getContentElement( 'info', 'linkType' ) )
							this.getElement().hide();
					}
				} ]
			},
			{
				id: 'target',
				requiredContent: 'a[target]', // This is not fully correct, because some target option requires JS.
				label: linkLang.target,
				title: linkLang.target,
				elements: [ {
					type: 'hbox',
					widths: [ '50%', '50%' ],
					children: [ {
						type: 'select',
						id: 'linkTargetType',
						label: commonLang.target,
						'default': '_blank',
						style: 'width : 100%;',
						'items': [
							[ commonLang.notSet, 'notSet' ],
							[ linkLang.targetFrame, 'frame' ],
							[ linkLang.targetPopup, 'popup' ],
							[ commonLang.targetNew, '_blank' ],
							[ commonLang.targetTop, '_top' ],
							[ commonLang.targetSelf, '_self' ],
							[ commonLang.targetParent, '_parent' ]
						],
						onChange: targetChanged,
						setup: function( data ) {
							if ( data.target )
								this.setValue( data.target.type || '_blank' );
							targetChanged.call( this );
						},
						commit: function( data ) {
							if ( !data.target )
								data.target = {};

							data.target.type = this.getValue();
						}
					},
					{
						type: 'text',
						id: 'linkTargetName',
						label: linkLang.targetFrameName,
						'default': '',
						setup: function( data ) {
							if ( data.target )
								this.setValue( data.target.name );
						},
						commit: function( data ) {
							if ( !data.target )
								data.target = {};

							data.target.name = this.getValue().replace( /([^\x00-\x7F]|\s)/gi, '' );
						}
					} ]
				},
				{
					type: 'vbox',
					width: '100%',
					align: 'center',
					padding: 2,
					id: 'popupFeatures',
					children: [ {
						type: 'fieldset',
						label: linkLang.popupFeatures,
						children: [ {
							type: 'hbox',
							children: [ {
								type: 'checkbox',
								id: 'resizable',
								label: linkLang.popupResizable,
								setup: setupPopupParams,
								commit: commitPopupParams,
								default:true
							},
							{
								type: 'checkbox',
								id: 'status',
								label: linkLang.popupStatusBar,
								setup: setupPopupParams,
								commit: commitPopupParams,
								

							} ]
						},
						{
							type: 'hbox',
							children: [ {
								type: 'checkbox',
								id: 'location',
								label: linkLang.popupLocationBar,
								setup: setupPopupParams,
								commit: commitPopupParams

							},
							{
								type: 'checkbox',
								id: 'toolbar',
								label: linkLang.popupToolbar,
								setup: setupPopupParams,
								commit: commitPopupParams

							} ]
						},
						{
							type: 'hbox',
							children: [ {
								type: 'checkbox',
								id: 'menubar',
								label: linkLang.popupMenuBar,
								setup: setupPopupParams,
								commit: commitPopupParams

							},
							{
								type: 'checkbox',
								id: 'fullscreen',
								label: linkLang.popupFullScreen,
								setup: setupPopupParams,
								commit: commitPopupParams

							} ]
						},
						{
							type: 'hbox',
							children: [ {
								type: 'checkbox',
								id: 'scrollbars',
								label: linkLang.popupScrollBars,
								setup: setupPopupParams,
								commit: commitPopupParams
,default:true
							},
							{
								type: 'checkbox',
								id: 'dependent',
								label: linkLang.popupDependent,
								setup: setupPopupParams,
								commit: commitPopupParams

							} ]
						},
						{
							type: 'hbox',
							children: [ {
								type: 'text',
								widths: [ '50%', '50%' ],
								labelLayout: 'horizontal',
								label: commonLang.width,
								id: 'width',
								setup: setupPopupParams,
								commit: commitPopupParams
,default:800
							},
							{
								type: 'text',
								labelLayout: 'horizontal',
								widths: [ '50%', '50%' ],
								label: linkLang.popupLeft,
								id: 'left',
								setup: setupPopupParams,
								commit: commitPopupParams
,default:230
							} ]
						},
						{
							type: 'hbox',
							children: [ {
								type: 'text',
								labelLayout: 'horizontal',
								widths: [ '50%', '50%' ],
								label: commonLang.height,
								id: 'height',
								setup: setupPopupParams,
								commit: commitPopupParams
,default:700
							},
							{
								type: 'text',
								labelLayout: 'horizontal',
								label: linkLang.popupTop,
								widths: [ '50%', '50%' ],
								id: 'top',
								setup: setupPopupParams,
								commit: commitPopupParams
,default:0
							} ]
						} ]
					} ]
				} ]
			},

			{ 
				id: 'upload',
				label: linkLang.upload,
				title: linkLang.upload,
				hidden: true,
				filebrowser: 'uploadButton',
				elements: [ {
					type: 'file',
					id: 'upload',
					label: commonLang.upload,
					style: 'height:40px',
					size: 29
				},
				{
					type: 'fileButton',
					id: 'uploadButton',
					label: commonLang.uploadSubmit,
					filebrowser: 'info:url',
					'for': [ 'upload', 'upload' ]
				} ]
			},
			{
				id: 'advanced',
				label: linkLang.advanced,
				title: linkLang.advanced,
				hidden:true
				,
				elements: [ {
					type: 'vbox',
					padding: 1,
					children: [ {
						type: 'hbox',
						widths: [ '45%', '35%', '20%' ],
						children: [ {
							type: 'text',
							id: 'advId',
							requiredContent: 'a[id]',
							label: linkLang.id,
							setup: setupAdvParams,
							commit: commitAdvParams
						},
						{
							type: 'select',
							id: 'advLangDir',
							requiredContent: 'a[dir]',
							label: linkLang.langDir,
							'default': '',
							style: 'width:110px',
							items: [
								[ commonLang.notSet, '' ],
								[ linkLang.langDirLTR, 'ltr' ],
								[ linkLang.langDirRTL, 'rtl' ]
							],
							setup: setupAdvParams,
							commit: commitAdvParams
						},
						{
							type: 'text',
							id: 'advAccessKey',
							requiredContent: 'a[accesskey]',
							width: '80px',
							label: linkLang.acccessKey,
							maxLength: 1,
							setup: setupAdvParams,
							commit: commitAdvParams

						} ]
					},
					{
						type: 'hbox',
						widths: [ '45%', '35%', '20%' ],
						children: [ {
							type: 'text',
							label: linkLang.name,
							id: 'advName',
							requiredContent: 'a[name]',
							setup: setupAdvParams,
							commit: commitAdvParams

						},
						{
							type: 'text',
							label: linkLang.langCode,
							id: 'advLangCode',
							requiredContent: 'a[lang]',
							width: '110px',
							'default': '',
							setup: setupAdvParams,
							commit: commitAdvParams

						},
						{
							type: 'text',
							label: linkLang.tabIndex,
							id: 'advTabIndex',
							requiredContent: 'a[tabindex]',
							width: '80px',
							maxLength: 5,
							setup: setupAdvParams,
							commit: commitAdvParams

						} ]
					} ]
				},
				{
					type: 'vbox',
					padding: 1,
					children: [ {
						type: 'hbox',
						widths: [ '45%', '55%' ],
						children: [ {
							type: 'text',
							label: linkLang.advisoryTitle,
							requiredContent: 'a[title]',
							'default': '',
							id: 'advTitle',
							setup: setupAdvParams,
							commit: commitAdvParams

						},
						{
							type: 'text',
							label: linkLang.advisoryContentType,
							requiredContent: 'a[type]',
							'default': '',
							id: 'advContentType',
							setup: setupAdvParams,
							commit: commitAdvParams

						} ]
					},
					{
						type: 'hbox',
						widths: [ '45%', '55%' ],
						children: [ {
							type: 'text',
							label: linkLang.cssClasses,
							requiredContent: 'a(cke-xyz)', // Random text like 'xyz' will check if all are allowed.
							'default': '',
							id: 'advCSSClasses',
							setup: setupAdvParams,
							commit: commitAdvParams

						},
						{
							type: 'text',
							label: linkLang.charset,
							requiredContent: 'a[charset]',
							'default': '',
							id: 'advCharset',
							setup: setupAdvParams,
							commit: commitAdvParams

						} ]
					},
					{
						type: 'hbox',
						widths: [ '45%', '55%' ],
						children: [ {
							type: 'text',
							label: linkLang.rel,
							requiredContent: 'a[rel]',
							'default': '',
							id: 'advRel',
							setup: setupAdvParams,
							commit: commitAdvParams
						},
						{
							type: 'text',
							label: linkLang.styles,
							requiredContent: 'a{cke-xyz}', // Random text like 'xyz' will check if all are allowed.
							'default': '',
							id: 'advStyles',
							validate: CKEDITOR.dialog.validate.inlineStyle( editor.lang.common.invalidInlineStyle ),
							setup: setupAdvParams,
							commit: commitAdvParams
						} ]
					} ]
				} ]
			} ],
			onShow: function() {
				var editor = this.getParentEditor(),
					selection = editor.getSelection(),
					element = null;

				// Fill in all the relevant fields if there's already one link selected.
				if ( ( element = plugin.getSelectedLink( editor ) ) && element.hasAttribute( 'href' ) ) {
					// Don't change selection if some element is already selected.
					// For example - don't destroy fake selection.
					if ( !selection.getSelectedElement() )
						selection.selectElement( element );
				} else {
					element = null;
				}

				var data = plugin.parseLinkAttributes( editor, element );

				// Record down the selected element in the dialog.
				this._.selectedElement = element;

				this.setupContent( data );
			},
			onOk: function() {
				var data = {};

				// Collect data from fields.
				this.commitContent( data );

				var selection = editor.getSelection(),
					attributes = plugin.getLinkAttributes( editor, data );

					 var selectedContent;

    if (selection.getType() == CKEDITOR.SELECTION_ELEMENT) {
        selectedContent = selection.getSelectedElement().getAttribute("alt");
          
  
    }
    else if (selection.getType() == CKEDITOR.SELECTION_TEXT) {
        selectedContent = selection.getSelectedText();
        
    }



// editor.getSelection().getSelectedElement().innerText = "hello me";



						if(data.type=='evidences'){
						data.target = {
							'dependent':"",
							'fullscreen':"",
							'height':"700",
							'left':"230",
							'location':"",
							'menubar':"",
							'name':"",
							'resizable':true,
							'scrollbars':true,
							'status':"",
							'toolbar':"",
							'top':"0",
							'type':"_blank",
							'width':"800"
						}
						attributes = {'removed':["target","onclick","data-cke-pa-onclick","data-cke-saved-name",
"id","dir",
"accessKey","name","lang",
"tabindex",
"title",
"type","class","charset","style",
"rel"],'set':{'data-cke-saved-href':data.evidences.path,'href':data.evidences.path,'target':'_blank'}};
						
						 
					}
							
	
				if ( !this._.selectedElement ) {
					var range = selection.getRanges()[ 0 ];
					
					// Use link URL as text with a collapsed cursor.
					if ( range.collapsed ) {
						// Short mailto link text view (#5736).
						var text = new CKEDITOR.dom.text( data.type == 'email' ?
							data.email.address : attributes.set[ 'data-cke-saved-href' ], editor.document );
						range.insertNode( text );
						range.selectNodeContents( text );
					}

					// Apply style.
					var style = new CKEDITOR.style( {
						element: 'a',
						attributes: attributes.set
					} );

					style.type = CKEDITOR.STYLE_INLINE; // need to override... dunno why.
					style.applyToRange( range, editor );

					range.select();

				} else {
					// We're only editing an existing link, so just overwrite the attributes.
					var element = this._.selectedElement,
						href = element.data( 'cke-saved-href' ),
						textView = element.getHtml();

					element.setAttributes( attributes.set );
					element.removeAttributes( attributes.removed );

					// Update text view when user changes protocol (#4612).
					if ( href == textView || data.type == 'email' && textView.indexOf( '@' ) != -1 ) {
						// Short mailto link text view (#5736).
						element.setHtml( data.type == 'email' ?
							data.email.address : attributes.set[ 'data-cke-saved-href' ] );

						// We changed the content, so need to select it again.
						selection.selectElement( element );
					}

					delete this._.selectedElement;
				}
			},
			onLoad: function() {
				if ( !editor.config.linkShowAdvancedTab )
					this.hidePage( 'advanced' ); //Hide Advanded tab.

				if ( !editor.config.linkShowTargetTab )
					this.hidePage( 'target' ); //Hide Target tab.
			},
			// Inital focus on 'url' field if link is of type URL.
			onFocus: function() {
				var linkType = this.getContentElement( 'info', 'linkType' ),
					urlField;

				if ( linkType && linkType.getValue() == 'url' ) {
					urlField = this.getContentElement( 'info', 'url' );
					urlField.select();
				}
			}
		};
	} );
} )();
