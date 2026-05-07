window.storage = {

    setSession: function (key, value) {
        sessionStorage.setItem(key, value);
    },

    getSession: function (key) {
        return sessionStorage.getItem(key);
    },

    removeSession: function (key) {
        sessionStorage.removeItem(key);
    }
};