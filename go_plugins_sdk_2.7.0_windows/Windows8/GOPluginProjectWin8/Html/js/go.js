var go = {
  executePlugin: function (plugin, action, args, callback) {
    var JSONParam = JSON.stringify({ plugin: plugin, action: action, args: args, callback: callback });
    window.external.notify(JSONParam);
  }
}

window.alert = function (arg) { window.external.notify("alert:" + arg); };