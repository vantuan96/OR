using System.Web.Optimization;

namespace Admin
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            BundleTable.Bundles.IgnoreList.Ignore(".intellisense.js", OptimizationMode.Always);
            BundleTable.Bundles.IgnoreList.Ignore("-vsdoc.js", OptimizationMode.Always);
            BundleTable.Bundles.IgnoreList.Ignore(".debug.js", OptimizationMode.Always);
            
            bundles.FileSetOrderList.Clear();
            #region Script bundles

            bundles.Add(new ScriptBundle("~/lang").Include("~/Assets/js/adr_global_lang.js"));

            bundles.Add(new ScriptBundle("~/javas").Include("~/Assets/js/jquery.min.js",
                                                            "~/Assets/js/jquery-ui.js",
                                                            "~/Assets/js/jquery.cookie.js",
                                                            "~/Assets/js/libaries/jquery.unobtrusive-ajax.min.js",
                                                            "~/Assets/js/libaries/jquery.validate.min.js",
                                                            //"~/Assets/js/libaries/jquery.validate.js",
                                                            "~/Assets/js/libaries/jquery.validate.unobtrusive.min.js",
                                                            "~/Assets/js/libaries/jquery.number.js",
                                                            "~/Assets/js/libaries/knockout-3.2.0.js",
                                                            "~/Assets/js/jquery.easing.1.3.min.js",
                                                            "~/Assets/bootstrap/js/bootstrap.min.js",
                                                            "~/Assets/lib/Parsley.js/dist/parsley.min.js",
                                                            "~/Assets/js/tinynav.min.js",
                                                            "~/Assets/lib/perfect-scrollbar/min/perfect-scrollbar-0.4.8.with-mousewheel.min.js",
                                                            "~/Assets/lib/colorbox/jquery.colorbox-min.js",
                                                            "~/Assets/lib/iOS-Overlay/js/iosOverlay.js",
                                                            "~/Assets/lib/moment-js/moment.min.js",
                                                            "~/Assets/lib/bootstrap-datepicker/js/bootstrap-datepicker.js",
                                                            "~/Assets/lib/bootstrap-timepicker/js/bootstrap-timepicker.js",
                                                            "~/Assets/lib/bootstrap-switch/build/js/bootstrap-switch.min.js",
                                                            "~/Assets/lib/noty/js/noty/packaged/jquery.noty.packaged.js",
                                                            "~/Assets/lib/powertip/jquery.powertip.min.js",
                                                            "~/Assets/lib/bootbox-js/bootbox.js",
                                                            "~/Assets/lib/jquery-treeview/jquery.treeview.js",
                                                            "~/Assets/lib/jquery-treeview/jquery.treeview.edit.js",
                                                            "~/Assets/lib/jquery-treeview/jquery.treeview.async.js",
                                                            //"~/Assets/js/jquery.ad-gallery.js",
                                                            //"~/Assets/lib/select2/select2.min.js",
                                                            "~/Assets/lib/select2_v4013/js/select2.min.js",
                                                            "~/Assets/js/customize.js",
                                                            "~/Assets/js/modernizr.custom.1.0.js",
                                                            "~/Assets/js/jquery-scrollTo.js",
                                                            "~/Assets/js/masonry.pkgd.js",
                                                            "~/Assets/js/adr_htmltemplate.js",
                                                            "~/Assets/js/validation/*.js",
                                                            "~/Assets/js/jquery.mCustomScrollbar.concat.min.js",
                                                            "~/Assets/js/apps/tisa_tooltips.js",
                                                            "~/Assets/lib/CLNDR/src/clndr.js",
                                                            "~/Assets/js/adr_common.js",
                                                            "~/Assets/js/ve_common.js",
                                                            "~/Assets/js/ve_lazy_submit.js",
                                                            "~/Assets/js/ve_tip.js",
                                                            "~/Assets/js/adr_modal.js",
                                                            "~/Assets/js/adr_notification.js",
                                                            //"~/Assets/js/drag_drop_booking.js"
                                                            "~/Assets/lib/underscore-js/underscore-min.js",
                                                            "~/Assets/js/apps/tisa_dashboard.js",
                                                            "~/Scripts/dropzone/dropzone.js",
                                                            "~/Assets/lib/lightbox2-master/js/lightbox.min.js",
                                                            "~/Assets/lib/ekko-lightbox/ekko-lightbox.js",
                                                            "~/Assets/lib/sortable/jquery-sortable-lists.js"
                                                            //Text avatar
                                                            ,"~/Assets/lib/textavatar/initial.min.js"
                                                            ));

            bundles.Add(new ScriptBundle("~/javas_x_editable").Include("~/Assets/lib/x-editable/bootstrap3-editable/js/bootstrap-editable.min.js",
                                                            "~/Assets/js/ve_settingsystem.js"));

            bundles.Add(new ScriptBundle("~/fctree").Include("~/Assets/lib/fancytree/js.cookie.min.js",
                                                            "~/Assets/lib/fancytree/jquery.fancytree.js",
                                                            "~/Assets/lib/fancytree/jquery.contextMenu.js",
                                                            "~/Assets/lib/fancytree/jquery.fancytree.filter.js",
                                                            "~/Assets/lib/fancytree/jquery.fancytree.table.js",
                                                            "~/Assets/lib/fancytree/jquery.fancytree.contextMenu.js",
                                                            "~/Assets/lib/fancytree/jquery.fancytree.persist.js"
                                                            ));

            bundles.Add(new ScriptBundle("~/Datatables").Include(
                                                           "~/Assets/lib/DataTables/media/js/jquery.dataTables.min.js",
                                                           "~/Assets/lib/DataTables/media/js/dataTables.bootstrap.js",
                                                           "~/Assets/lib/DataTables/extensions/TableTools/js/dataTables.tableTools.min.js",
                                                           "~/Assets/lib/DataTables/extensions/Scroller/js/dataTables.scroller.min.js"
                                                           ));

            bundles.Add(new ScriptBundle("~/javas_searchfrm").Include("~/Assets/lib/bootstrap-daterangepicker/daterangepicker.js",
                                                                        "~/Assets/lib/jquery-datetimepicker/jquery.datetimepicker.js",
                                                                        "~/Assets/lib/bootstrap-multiselect/bootstrap-multiselect.js",
                                                                        "~/Assets/lib/timepicker/1.3.5/jquery.timepicker.min.js"
                                                                        ));

            bundles.Add(new ScriptBundle("~/loginvalidator").Include("~/Assets/js/jquery.min.js",
                "~/Assets/js/libaries/jquery.unobtrusive-ajax.min.js",
                "~/Assets/js/libaries/jquery.validate.min.js",
                //"~/Assets/js/libaries/jquery.validate.js",
                "~/Assets/js/libaries/jquery.validate.unobtrusive.min.js",
                "~/Assets/js/validation/EmailAddressVE.js"));

            bundles.Add(new ScriptBundle("~/js_report").Include("~/Assets/js/vg_report.js"));

            bundles.Add(new ScriptBundle("~/usermanager").Include("~/Assets/js/ve_user.js"));
            bundles.Add(new ScriptBundle("~/surgery-manager").Include("~/Assets/js/ve_surgery.js"));
            bundles.Add(new ScriptBundle("~/surgery-reg-manager").Include("~/Assets/js/ve_reg_surgery.js"));
            bundles.Add(new ScriptBundle("~/roommanager").Include("~/Assets/js/vm_room.js"));
            bundles.Add(new ScriptBundle("~/menu_mngt").Include("~/Assets/js/vg_menu_mngt.js"));
            bundles.Add(new ScriptBundle("~/address_mngt").Include("~/Assets/js/vg_address_mngt.js"));
            bundles.Add(new ScriptBundle("~/route_mngt").Include("~/Assets/js/vg_route_mngt.js"));
            bundles.Add(new ScriptBundle("~/gallery_mngt").Include(
                "~/Assets/js/vg_gallery_mngt.js",
                "~/Assets/lib/jQuery-multisortable/jquery.multisortable.js"));
            bundles.Add(new ScriptBundle("~/product_mngt").Include("~/Assets/js/vg_product_mngt.js"));
            bundles.Add(new ScriptBundle("~/ckeditor").Include("~/Assets/lib/ckeditor/ckeditor.js"));

            //bundles.Add(new ScriptBundle("~/bootstrap_multiselectjs").Include("~/Assets/lib/bootstrap-multiselect/bootstrap-munltiselect.js"));

            bundles.Add(new ScriptBundle("~/nvd3charts_js").Include(
                    "~/Assets/lib/d3/d3.min.js",
                    "~/Assets/lib/novus-nvd3/nv.d3.min.js"
            ));

            bundles.Add(new ScriptBundle("~/HighCharts_js").Include(
                    "~/Assets/lib/highcharts/HighCharts.js"
            ));

            bundles.Add(new ScriptBundle("~/NewHightChart").Include(
                 "~/Assets/lib/highcharts/HighCharts.js",
                 "~/Assets/lib/highcharts/modules/exporting.js",
                 "~/Assets/lib/highcharts/modules/series-label.js",
                 "~/Assets/lib/highcharts/modules/data.js",
                 "~/Assets/lib/highcharts/modules/drilldown.js"
         ));
            bundles.Add(new ScriptBundle("~/printsurgeryJS").Include("~/Assets/js/JsBarcode.all.min.js"));
            //linht
            bundles.Add(new ScriptBundle("~/shareJS").Include("~/Assets/js/share-expand.js"));
            #endregion Script bundles

            #region Style bundles

            bundles.Add(new StyleBundle("~/basecss").Include("~/Assets/css/jquery-ui.css",
                                                        "~/Assets/icons/font-awesome/css/font-awesome.min.css",
                                                        "~/Assets/bootstrap/css/bootstrap.min.css",
                                                        "~/Assets/icons/ionicons/css/ionicons.min.css",
                                                        "~/Assets/lib/bootstrap-datepicker/css/datepicker3.css",
                                                        "~/Assets/lib/x-editable/bootstrap3-editable/css/bootstrap-editable.css",
                                                        "~/Assets/lib/bootstrap-switch/build/css/bootstrap3/bootstrap-switch.css",
                                                        //"~/Assets/lib/select2/select2.css",
                                                        "~/Assets/lib/select2_v4013/css/select2.css",
                                                        "~/Assets/css/Assets/colorbox/colorbox.css",
                                                        "~/Assets/lib/jquery-treeview/css/jquery.treeview.css",
                                                        "~/Assets/lib/jquery-treeview/css/screen.css",
                                                        //"~/Assets/css/jquery.ad-gallery.css",
                                                        "~/Assets/css/jquery.mCustomScrollbar.css",
                                                        "~/Assets/lib/novus-nvd3/nv.d3.min.css",
                                                        "~/Assets/css/style.css",
                                                        "~/Assets/css/customize.css",
                                                        "~/Assets/css/customize_menu.css",
                                                        "~/Assets/css/main_v2.css",
                                                        "~/Assets/css/adr.notification.css",
                                                        "~/Assets/css/ie.css",
                                                        "~/Scripts/dropzone/basic.css",
                                                        "~/Scripts/dropzone/dropzone.css",
                                                        "~/Assets/lib/lightbox2-master/css/lightbox.css",
                                                        "~/Assets/lib/ekko-lightbox/ekko-lightbox.css",
                                                        "~/Assets/lib/fancytree/skin-win8/ui.fancytree.css",
                                                        "~/Assets/lib/fancytree/skin-win8/jquery.contextMenu.css"
                                                        ));

            bundles.Add(new StyleBundle("~/extcss").Include("~/Assets/css/style.css",
                                                        "~/Assets/css/customize.css",
                                                        "~/Assets/css/customize_menu.css",
                                                        "~/Assets/css/adr_notification.css",
                                                        "~/Assets/css/main_v2.css",
                                                        "~/Assets/css/update_022022.css",
                                                        "~/Assets/css/ie.css",
                                                          "~/Assets/css/DropdownSelectMoreApp.css"
                                                        ));
            bundles.Add(new StyleBundle("~/authbasecss").Include("~/Assets/bootstrap/css/bootstrap.min.css",
                                                       "~/Assets/css/login.css"));

            bundles.Add(new StyleBundle("~/sortablecss").Include("~/Assets/css/sortable.css"));

            bundles.Add(new StyleBundle("~/styles_searchfrm").Include("~/Assets/lib/bootstrap-daterangepicker/daterangepicker-bs3.css",
                                                                        "~/Assets/lib/jquery-datetimepicker/jquery.datetimepicker.css",
                                                                        "~/Assets/lib/timepicker/1.3.5/jquery.timepicker.min.css",
                                                                        "~/Assets/css/bootstrap-multiselect.css"));
            bundles.Add(new StyleBundle("~/printsurgery").Include("~/Assets/css/ReceiptSurgery.css"));

            #endregion Style bundles
        }
    }
}