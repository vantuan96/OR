// Register a new CKEditor plugin.
// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.resourceManager.html#add
CKEDITOR.plugins.add( 'vgslider',
{
    onLoad: function () {
        var lc_path = this.path;
        CKEDITOR.addCss(            
            '.cke-gallery {' +
            '   background-color:#efefef;' +
            '   height: 150px;' +
            '   min-width: 30px;' +
            '   text-align: center;' +
            '}'+
            '.cke-gallery > img {' +
            '   height: 100%;' +
            '}' +
            '.cke-gallery-preview {' +
            '   height: 100%;' +
            '   background: url(\'' + lc_path + 'images/ckesliderpreview.png\')  no-repeat center center' +
            '}');
    },
	// The plugin initialization logic goes inside this method.
	// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.pluginDefinition.html#init
	init: function( editor )
	{
		// Create an editor command that stores the dialog initialization command.
		// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.command.html
		// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dialogCommand.html
		editor.addCommand( 'vgsliderDialog', new CKEDITOR.dialogCommand( 'vgsliderDialog' ) );
 
		// Create a toolbar button that executes the plugin command defined above.
		// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.ui.html#addButton
		editor.ui.addButton( 'vgslider',
		{
			// Toolbar button tooltip.
			label: 'Ảnh slide',
			// Reference to the plugin command name.
			command: 'vgsliderDialog',
			// Button's icon file path.
			icon: this.path + 'images/icon.png'
		} );
 
		var lc_path = this.path;

		// Add a new dialog window definition containing all UI elements and listeners.
		// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dialog.html#.add
		// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dialog.dialogDefinition.html
		CKEDITOR.dialog.add( 'vgsliderDialog', function( editor )
		{
			return {
				// Basic properties of the dialog window: title, minimum size.
				// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dialog.dialogDefinition.html
				title : 'Chèn album ảnh slide',
				minWidth : 400,
				minHeight : 40,
				// Dialog window contents.
				// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dialog.definition.content.html
				contents :
				[
					{
						// Definition of the Settings dialog window tab (page) with its id, label and contents.
						// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dialog.contentDefinition.html
						id : 'general',
						label : 'Settings',
						elements :
						[
							//// Dialog window UI element: HTML code field.
							//// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.ui.dialog.html.html
							//{
							//	type : 'html',
							//	// HTML code to be shown inside the field.
							//	// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.ui.dialog.html.html#constructor
							//	html : 'This dialog window lets you create simple links for your website.'
							//},
							// Dialog window UI element: a textarea field for the link text.
							// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.ui.dialog.textarea.html
							//{
							//	type : 'textarea',
							//	id : 'contents',
							//	// Text that labels the field.
							//	// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.ui.dialog.labeledElement.html#constructor
							//	label : 'Displayed Text',
							//	// Validation checking whether the field is not empty.
							//	validate : CKEDITOR.dialog.validate.notEmpty( 'The Displayed Text field cannot be empty.' ),
							//	// This field is required.
							//	required : true,
							//	// Function to be run when the commitContent method of the parent dialog window is called.
							//	// Get the value of this field and save it in the data object attribute.
							//	// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dom.element.html#getValue
							//	commit : function( data )
							//	{
							//		data.contents = this.getValue();
							//	}
							//},
							// Dialog window UI element: a text input field for the link URL.
							// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.ui.dialog.textInput.html
							{
								type : 'text',
								id: 'galleryId',
								label : 'Mã gallery',
								validate: CKEDITOR.dialog.validate.notEmpty('Thông tin bắt buộc.'),
								required : true,
								commit : function( data )
								{
									data.galleryId = this.getValue();
								}
							},
                            //{
                            //    type: 'text',
                            //    id: 'width',
                            //    label: 'Chiều dài',
                            //    validate: CKEDITOR.dialog.validate.integer('Phải là kiểu số nguyên.')
                            //        || CKEDITOR.dialog.validate.notEmpty('Thông tin bắt buộc.'),
                            //    required: true,
                            //    commit: function (data) {
                            //        data.width = this.getValue();
                            //    }
                            //},
                            //{
                            //    type: 'text',
                            //    id: 'height',
                            //    label: 'Chiều cao',
                            //    validate: CKEDITOR.dialog.validate.integer('Phải là kiểu số nguyên.') 
                            //        || CKEDITOR.dialog.validate.notEmpty('Thông tin bắt buộc.'),
                            //    required: true,
                            //    commit: function (data) {
                            //        data.height = this.getValue();
                            //    }
                            //},
							// Dialog window UI element: a selection field with link styles.
							// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.ui.dialog.select.html
							//{
							//	type : 'select',
							//	id : 'position',
							//	label : 'Vị trí',
							//	// Items that will appear inside the selection field, in pairs of displayed text and value.
							//	// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.ui.dialog.select.html#constructor
							//	items : 
							//	[
							//		[ 'Trái', 'l' ],
							//		[ 'Giữa', 'c' ],
							//		[ 'Phải', 'r' ]
							//	],
							//	commit : function( data )
							//	{
							//	    data.position = this.getValue();
							//	}
							//}
							//,
							//// Dialog window UI element: a checkbox for opening in a new page.
							//// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.ui.dialog.checkbox.html
							//{
							//	type : 'checkbox',
							//	id : 'newPage',
							//	label : 'Opens in a new page',
							//	// Default value.
							//	// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.ui.dialog.checkbox.html#constructor
							//	'default' : true,
							//	commit : function( data )
							//	{
							//		data.newPage = this.getValue();
							//	}
							//}
						]
					}
				],
				
				onOk : function()
				{
					// Create a link element and an object that will store the data entered in the dialog window.
					// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dom.document.html#createElement
					var dialog = this,
						data = {},
						gallery = editor.document.createElement( 'div' );
					// Populate the data object with data entered in the dialog window.
					// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dialog.html#commitContent
					this.commitContent( data );

					// Set the URL (href attribute) of the link element.
					// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dom.element.html#setAttribute
					gallery.setAttribute('data-gallery-id', data.galleryId);
					gallery.setAttribute('class', 'cke-gallery');
					gallery.setAttribute('contenteditable', 'false');
					var img = editor.document.createElement('p');
					img.setAttribute('class', 'cke-gallery-preview');
					//img.setStyle('height', '100%');
					//img.setStyle('background', ' url(\'' + lc_path + 'images/ckesliderpreview.png\')  no-repeat center center');

					// In case the "newPage" checkbox was checked, set target=_blank for the link element.
					//if ( data.newPage )
				    //	link.setAttribute( 'target', '_blank' );

					//gallery.setStyle('width', data.width);
					//gallery.setStyle('height', data.height);

					// Set the style selected for the link, if applied.
					// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dom.element.html#setStyle
					//switch (data.position)
					//{
					//	case 'l' :
					//	    gallery.setStyle('float', 'left');
					//	break;
					//	case 'c' :
					//	    gallery.setStyle('float', 'none');
					//	break;
					//	case 'r' :
					//	    gallery.setStyle('float', 'right');
					//	break;
					//}

					// Insert the Displayed Text entered in the dialog window into the link element.
					// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dom.element.html#setHtml
					//gallery.setHtml("{Nội dung ảnh slide}");
					gallery.append(img);

					// Insert the link element into the current cursor position in the editor.
					// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.editor.html#insertElement
					editor.insertElement(gallery);
				}
			};
		} );
	}
} );