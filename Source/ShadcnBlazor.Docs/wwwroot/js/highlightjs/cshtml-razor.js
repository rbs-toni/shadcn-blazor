﻿hljs.registerLanguage("cshtml-razor", (() => {
  "use strict"; return n => {
    var e = "built_in", s = {}, a = { begin: "}", className: e, endsParent: !0 }, i = {
      begin: "{",
      end: "}", contains: [n.QUOTE_STRING_MODE, "self"]
    }, r = n.COMMENT("@\\*", "\\*@", {
      relevance: 10
    }), g = {
      begin: "@[A-Za-z0-9\\._:-]+", returnBegin: !0,
      end: "(\\r|\\n|<|\\s|\"|')", subLanguage: "csharp", contains: [{
        begin: "@", className: e
      }, { begin: "\\[", end: "\\]", skip: !0 }, { begin: "\\(", end: "\\)", skip: !0 }], returnEnd: !0
    }, t = {
      begin: "[@]{0,1}<text>", returnBegin: !0, end: "</text>", returnEnd: !0,
      subLanguage: "cshtml-razor", contains: [{ begin: "[@]{0,1}<text>", className: e }, {
        begin: "</text>", className: e, endsParent: !0
      }]
    }, c = {
      begin: "@\\(", end: "\\)",
      returnBegin: !0, returnEnd: !0, subLanguage: "csharp", contains: [{
        begin: "@\\(",
        className: e
      }, {
        begin: "\\(", end: "\\)", subLanguage: "csharp",
        contains: [n.QUOTE_STRING_MODE, "self", t]
      }, t, {
        begin: "\\)", className: e,
        endsParent: !0
      }]
    }, b = ((n, e) => {
      var s = {
        endsWithParent: !0, illegal: /</, relevance: 0,
        contains: [{ className: "attr", begin: "[A-Za-z0-9\\._:-]+", relevance: 0 }, {
          begin: /=\s*/, relevance: 0, contains: [{
            className: "string", variants: [{
              begin: /"/,
              end: /"/, contains: e
            }, { begin: /'/, end: /'/, contains: e }, { begin: /[^\s"'=<>`]+/ }]
          }]
        }]
      }
      ; return [{
        className: "meta", begin: "<!DOCTYPE", end: ">", relevance: 10, contains: [{
          begin: "\\[", end: "\\]"
        }]
      }, n.COMMENT("\x3c!--", "--\x3e", { relevance: 10 }), {
        begin: "<\\!\\[CDATA\\[", end: "\\]\\]>", relevance: 10
      }, {
        className: "meta",
        begin: /<\?xml/, end: /\?>/, relevance: 10
      }, {
        className: "tag",
        begin: "<style(?=\\s|>|$)", end: ">", keywords: { name: "style" }, contains: [s], starts: {
          end: "</style>", returnEnd: !0, subLanguage: ["css", "xml"]
        }
      }, {
        className: "tag",
        begin: "<script(?=\\s|>|$)", end: ">", keywords: { name: "script" }, contains: [s],
        starts: {
          end: "<\/script>", returnEnd: !0,
          subLanguage: ["actionscript", "javascript", "handlebars", "xml"]
        }
      }, {
        className: "tag",
        begin: "</?", end: "/?>", contains: [{
          className: "name", begin: /[^\/><\s]+/, relevance: 0
        }, s]
      }].concat(e)
    })(n, [g, c]), l = "^\\s*@(page|model|using|inherits|inject|layout)", u = {
      begin: l + "[^\\r\\n{\\(]*$", end: "$", returnBegin: !0, returnEnd: !0, contains: [{
        begin: l, className: e
      }, {
        variants: [{ begin: "\\r|\\n", endsParent: !0 }, {
          begin: "\\s[^\\r\\n]+", end: "$"
        }, { begin: "$" }], className: "type", endsParent: !0
      }]
    }, d = {
      variants: [{ begin: "@\\{", end: "}" }, { begin: "@code\\s*\\{", end: "}" }],
      returnBegin: !0, returnEnd: !0, subLanguage: "csharp", contains: [{
        begin: "@(code\\s*)?\\{", className: e
      }, s, {
        begin: "{", end: "}", contains: ["self"],
        skip: !0
      }, a]
    }, o = {
      begin: "^\\s*@helper[\\s]*[^{]+[\\s]*{", returnBegin: !0,
      returnEnd: !0, end: "}", subLanguage: "cshtml-razor", contains: [{
        begin: "@helper",
        className: e
      }, { begin: "{", className: e }, a]
    }, m = [{
      begin: "@for[\\s]*\\([^{]+[\\s]*{",
      end: "}"
    }, { begin: "@if[\\s]*\\([^{]+[\\s]*{", end: "}" }, {
      begin: "@switch[\\s]*\\([^{]+[\\s]*{", end: "}"
    }, {
      begin: "@while[\\s]*\\([^{]+[\\s]*{", end: "}"
    }, {
      begin: "@using[\\s]*\\([^{]+[\\s]*{", end: "}"
    }, {
      begin: "@lock[\\s]*\\([^{]+[\\s]*{", end: "}"
    }, {
      begin: "@foreach[\\s]*\\([^{]+[\\s]*{", end: "}"
    }], N = {
      variants: m, returnBegin: !0,
      returnEnd: !0, subLanguage: "csharp", contains: [{
        variants: m.map((n => ({
          begin: n.begin
        }))), returnBegin: !0, contains: [{ begin: "@", className: e }, {
          variants: m.map((n => ({
            begin: n.begin.substr(1, n.begin.length - 2)
          }))), subLanguage: "csharp"
        }, {
          begin: "{",
          className: e
        }]
      }, s, {
        variants: [{ begin: "}[\\s]*else\\sif[\\s]*\\([^{]+[\\s]*{" }, {
          begin: "}[\\s]*else[\\s]*{"
        }], returnBegin: !0, contains: [{ begin: "}", className: e }, {
          variants: [{ begin: "[\\s]*else\\sif[\\s]*\\([^{]+[\\s]*{" }, {
            begin: "[\\s]*else[\\s]*"
          }], subLanguage: "csharp"
        }, { begin: "{", className: e }]
      }, i, a]
    }, h = {
      begin: "@try[\\s]*{", end: "}", returnBegin: !0, returnEnd: !0,
      subLanguage: "csharp", contains: [{ begin: "@", className: e }, {
        begin: "try[\\s]*{",
        subLanguage: "csharp"
      }, {
        variants: [{
          begin: "}[\\s]*catch[\\s]*\\([^\\)]+\\)[\\s]*{"
        }, { begin: "}[\\s]*finally[\\s]*{" }], returnBegin: !0, contains: [{
          begin: "}",
          className: e
        }, {
          variants: [{ begin: "[\\s]*catch[\\s]*\\([^\\)]+\\)[\\s]*" }, {
            begin: "[\\s]*finally[\\s]*"
          }], subLanguage: "csharp"
        }, { begin: "{", className: e }]
      }, s, i, a]
    }, p = "@section[\\s]+[a-zA-Z0-9]+[\\s]*{", v = [u, o, d, N, {
      begin: p,
      returnBegin: !0, returnEnd: !0, end: "}", subLanguage: "cshtml-razor", contains: [{
        begin: p, className: e
      }, i, a]
    }, {
      begin: "@await ", returnBegin: !0, subLanguage: "csharp",
        end: "(\\r|\\n|<|\\s)", contains: [{ begin: "@await ", className: e }, {
          begin: "[<\\r\\n]", endsParent: !0
        }]
      }, h, {
        variants: [{ begin: "@@" }, {
          begin: "[a-zA-Z]+@"
        }], skip: !0
      }, t, r, c, {
        className: "meta", begin: "<!DOCTYPE", end: ">", relevance: 10,
        contains: [{ begin: "\\[", end: "\\]" }]
      }, {
        begin: "<\\!\\[CDATA\\[", end: "\\]\\]>",
        relevance: 10
      }].concat(b); return [d, N, h].forEach((n => {
        var e = v.filter((e => e !== n)), a = n.contains.indexOf(s)
          ; n.contains.splice.apply(n.contains, [a, 1].concat(e))
      })), {
        aliases: ["cshtml", "razor", "razor-cshtml", "cshtml-razor"], contains: v
      }
  }
})());