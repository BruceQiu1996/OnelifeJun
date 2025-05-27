window.localStorageInterop = {
  setItem: function (key, value) {
    localStorage.setItem(key, value);
  },
  getItem: function (key) {
    return localStorage.getItem(key);
  },
  removeItem: function (key) {
    localStorage.removeItem(key);
  },
  clear: function () {
    localStorage.clear();
  }
};

window.getScrollHeight = function (element) {
    return element.scrollHeight;
};

window.getScrollTop = function (element) {
    return element.scrollTop;
};

window.getClientHeight = function (element) {
    return element.clientHeight;
};