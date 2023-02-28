var qcsContact = {
    hotline: "04.39756699 ext 5555",
    email: "helpdesk-vcr@vincom.com.vn",
    skype: "kien.cd",
    name: "IT-Soft Cao Đức Kiên",
    mobile: "0988439593"
};

var newlinechar = '%0D%0A';
var psText = "Mọi vướng mắc về kỹ thuật của phần mềm QCS đề nghị liên lạc với bộ phận IT-VCR. Hotline: " + qcsContact.hotline + ", email: " + qcsContact.email + ", skype: " + qcsContact.skype + ", " + qcsContact.name + " - " + qcsContact.mobile;

var qcsMailTemplate = {
    capreport: {
        title: "[QCS]-Hệ thống KSCL thông báo danh sách lỗi cần khắc phục",
        content: 'Kính gửi GĐ BQL {sitename}!' 
            + newlinechar + 'Bộ phận KSCL vừa tiến hành kiểm tra tại TTTM {sitename} từ ngày {startdate} đến ngày {enddate}. BQL tiến hành khắc phục lỗi theo danh sách lỗi ({url}) và cập nhật kết quả khắc phục tại ({urlfeed})'
            + newlinechar + 'Danh sách lỗi (Menu(QCS) => Báo cáo => Báo cáo C.A.P): {url}'
            + newlinechar + newlinechar + '(Nếu có thắc mắc về nghiệp vụ đề nghị liên lạc trực tiếp với KSCL {fullname})'
    },
    capdonefix: {
        title: "[QCS]-Hệ thống KSCL thông báo danh sách lỗi đã hoàn thành khắc phục!",
        content: 'Kính gửi KSCL!'
            + newlinechar + 'BQL TTTM {sitename} cập nhật tình trạng lỗi theo danh sách trong hệ thống QCS ở đường link bên dưới.'
            + newlinechar + 'Đường link vào khắc phục lỗi (Menu(QCS) => Kiểm tra đánh giá => Quản lý lỗi vi phạm): {url}'
    },
    cap: {
        title: "[QCS]-Hệ thống KSCL thông báo danh sách lỗi cần phê duyệt gia hạn!",
        content: 'Kính gửi GĐ Miền!'
            + newlinechar + 'BQL TTTM {sitename} Đề nghị GĐ Miền phê duyệt danh sách các lỗi theo danh sách trong hệ thống QCS bằng cách vào đường link bên dưới.'
            + newlinechar + 'Danh sách lỗi xin phê duyệt (Menu(QCS) => Kiểm tra đánh giá => Quản lý lỗi vi phạm): {url}'
            + newlinechar + 'Kính mong GĐ miền phê duyệt!'
    },
    checklist: {
        title: "[QCS]-Hệ thống KSCL thông báo kết quả Checklist!",
        content: 'Kính gửi GĐ BQL {sitename}!'
            + newlinechar + 'Bộ phận KSCL vừa tiến hành kiểm tra tại TTTM {sitename} từ ngày {startdate} đến ngày {enddate}. BQL có thể kiểm tra kết quả tại báo cáo Checklist'
			+ newlinechar + 'Kết quả checklist (Menu(QCS) => Báo cáo => Báo cáo checklist bộ phận/hạng mục): {url}'
			+ newlinechar + newlinechar + '(Nếu có thắc mắc về nghiệp vụ đề nghị liên lạc trực tiếp với KSCL {fullname})'
    },

};

$(function () {
    if ($('.sendoutlook').length > 0)
    {
        $('.sendoutlook').click(function (e) {
            e.preventDefault();
            var templateName = $(this).data('mail-template');
            var templateObj = qcsMailTemplate[templateName];
            var regexMailReplace = /{([^ ]*)}/gi;
            
            var content = templateObj.content;
            var result;

            while (result = regexMailReplace.exec(templateObj.content)) {
                var paramname = result[1];
                var replacement = $(this).data('mail-' + paramname);
                if (replacement == undefined || replacement == null) replacement = '';

                content = content.replace(new RegExp('{' + paramname + '}', 'g'), replacement);
            }

            if (content.length + psText.length < 2000)
                content += newlinechar + psText;

            content = content.replace(new RegExp('&', 'g'), '%26');
            window.location.href = "mailto:?subject=" + templateObj.title + "&body=" + content;
        });
    }
})