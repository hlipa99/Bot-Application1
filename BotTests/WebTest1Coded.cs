﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BotTests {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.WebTesting;
    
    
    public class WebTest1Coded : WebTest {
        
        public WebTest1Coded() {
            this.PreAuthenticate = true;
            this.Proxy = "default";
        }
        
        public override IEnumerator<WebTestRequest> GetRequestEnumerator() {
            // //exchanging secret with token
            this.AddCommentToResult("//exchanging secret with token");

            WebTestRequest request1 = new WebTestRequest("https://directline.botframework.com/v3/directline/tokens/generate");
            request1.Method = "POST";
            request1.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            request1.Headers.Add(new WebTestRequestHeader("Authorization", "Bearer IK1o_f1fBYc.cwA.LZs.ZrbtNGSTAYzcLqc6DPbq_0bBOV_zzyMSYn7KCaoVdpI"));
            FormPostHttpBody request1Body = new FormPostHttpBody();
            request1.Body = request1Body;
            yield return request1;
            request1 = null;
        }
    }
}
