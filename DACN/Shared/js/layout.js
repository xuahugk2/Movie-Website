var pre = '';
function active(id) {
    var current = document.getElementById(id);
    if (current.classList.contains('active') === false) {
        current.classList.add('active')
    }
    if (pre.length !== 0) {
        var preEle = document.getElementById(pre);
        if (preEle.classList.contains('active') === true) {
            preEle.classList.remove('active')
        }
    }
}