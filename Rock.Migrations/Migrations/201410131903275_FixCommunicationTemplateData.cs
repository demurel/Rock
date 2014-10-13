// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
namespace Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    ///
    /// </summary>
    public partial class FixCommunicationTemplateData : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            Sql( @"
-- Add back the communication template content
UPDATE [CommunicationTemplate] 
SET [MediumDataJson] = '{ ""HtmlMessage"": ""<meta content=\""text/html; charset=utf-8\"" http-equiv=\""Content-Type\"" />\n<meta content=\""width=device-width\"" name=\""viewport\"" />\n<style type=\""text/css\"">td, h1, h2, h3, h4, h5, h6, p, a {\n    font-family: ''Helvetica'', ''Arial'', sans-serif; \n    line-height: 1.2; \n}\n\nh1, h2, h3, h4 {\n    color: #777;\n}\n\np {\n    color: #777;\n}\n\nem {\n    color: #777;\n}\n\na {\n    color: #2ba6cb; \n    text-decoration: none;\n}\n\n.btn a {\n    line-height: 1;\n    margin: 0;\n    padding: 0;\n}\n\na:hover {\ncolor: #2795b6 !important;\n}\na:active {\ncolor: #2795b6 !important;\n}\na:visited {\ncolor: #2ba6cb !important;\n}\nh1 a:active {\ncolor: #2ba6cb !important;\n}\nh2 a:active {\ncolor: #2ba6cb !important;\n}\nh3 a:active {\ncolor: #2ba6cb !important;\n}\nh4 a:active {\ncolor: #2ba6cb !important;\n}\nh5 a:active {\ncolor: #2ba6cb !important;\n}\nh6 a:active {\ncolor: #2ba6cb !important;\n}\nh1 a:visited {\ncolor: #2ba6cb !important;\n}\nh2 a:visited {\ncolor: #2ba6cb !important;\n}\nh3 a:visited {\ncolor: #2ba6cb !important;\n}\nh4 a:visited {\ncolor: #2ba6cb !important;\n}\nh5 a:visited {\ncolor: #2ba6cb !important;\n}\nh6 a:visited {\ncolor: #2ba6cb !important;\n}\ntable.button:hover td {\nbackground: #2795b6 !important;\n}\ntable.button:visited td {\nbackground: #2795b6 !important;\n}\ntable.button:active td {\nbackground: #2795b6 !important;\n}\ntable.button:hover td a {\ncolor: #fff !important;\n}\ntable.button:visited td a {\ncolor: #fff !important;\n}\ntable.button:active td a {\ncolor: #fff !important;\n}\ntable.button:hover td {\nbackground: #2795b6 !important;\n}\ntable.tiny-button:hover td {\nbackground: #2795b6 !important;\n}\ntable.small-button:hover td {\nbackground: #2795b6 !important;\n}\ntable.medium-button:hover td {\nbackground: #2795b6 !important;\n}\ntable.large-button:hover td {\nbackground: #2795b6 !important;\n}\ntable.button:hover td a {\ncolor: #ffffff !important;\n}\ntable.button:active td a {\ncolor: #ffffff !important;\n}\ntable.button td a:visited {\ncolor: #ffffff !important;\n}\ntable.tiny-button:hover td a {\ncolor: #ffffff !important;\n}\ntable.tiny-button:active td a {\ncolor: #ffffff !important;\n}\ntable.tiny-button td a:visited {\ncolor: #ffffff !important;\n}\ntable.small-button:hover td a {\ncolor: #ffffff !important;\n}\ntable.small-button:active td a {\ncolor: #ffffff !important;\n}\ntable.small-button td a:visited {\ncolor: #ffffff !important;\n}\ntable.medium-button:hover td a {\ncolor: #ffffff !important;\n}\ntable.medium-button:active td a {\ncolor: #ffffff !important;\n}\ntable.medium-button td a:visited {\ncolor: #ffffff !important;\n}\ntable.large-button:hover td a {\ncolor: #ffffff !important;\n}\ntable.large-button:active td a {\ncolor: #ffffff !important;\n}\ntable.large-button td a:visited {\ncolor: #ffffff !important;\n}\ntable.secondary:hover td {\nbackground: #d0d0d0 !important; color: #555;\n}\ntable.secondary:hover td a {\ncolor: #555 !important;\n}\ntable.secondary td a:visited {\ncolor: #555 !important;\n}\ntable.secondary:active td a {\ncolor: #555 !important;\n}\ntable.success:hover td {\nbackground: #457a1a !important;\n}\ntable.alert:hover td {\nbackground: #970b0e !important;\n}\ntable.facebook:hover td {\nbackground: #2d4473 !important;\n}\ntable.twitter:hover td {\nbackground: #0087bb !important;\n}\ntable.google-plus:hover td {\nbackground: #CC0000 !important;\n}\n@media only screen and (max-width: 600px) {\n  table[class=\""body\""] img {\n    width: auto !important; height: auto !important;\n  }\n  table[class=\""body\""] center {\n    min-width: 0 !important;\n  }\n  table[class=\""body\""] .container {\n    width: 95% !important;\n  }\n  table[class=\""body\""] .row {\n    width: 100% !important; display: block !important;\n  }\n  table[class=\""body\""] .wrapper {\n    display: block !important; padding-right: 0 !important;\n  }\n  table[class=\""body\""] .columns {\n    table-layout: fixed !important; float: none !important; width: 100% !important; padding-right: 0px !important; padding-left: 0px !important; display: block !important;\n  }\n  table[class=\""body\""] .column {\n    table-layout: fixed !important; float: none !important; width: 100% !important; padding-right: 0px !important; padding-left: 0px !important; display: block !important;\n  }\n  table[class=\""body\""] .wrapper.first .columns {\n    display: table !important;\n  }\n  table[class=\""body\""] .wrapper.first .column {\n    display: table !important;\n  }\n  table[class=\""body\""] table.columns td {\n    width: 100% !important;\n  }\n  table[class=\""body\""] table.column td {\n    width: 100% !important;\n  }\n  table[class=\""body\""] td.offset-by-one {\n    padding-left: 0 !important;\n  }\n  table[class=\""body\""] td.offset-by-two {\n    padding-left: 0 !important;\n  }\n  table[class=\""body\""] td.offset-by-three {\n    padding-left: 0 !important;\n  }\n  table[class=\""body\""] td.offset-by-four {\n    padding-left: 0 !important;\n  }\n  table[class=\""body\""] td.offset-by-five {\n    padding-left: 0 !important;\n  }\n  table[class=\""body\""] td.offset-by-six {\n    padding-left: 0 !important;\n  }\n  table[class=\""body\""] td.offset-by-seven {\n    padding-left: 0 !important;\n  }\n  table[class=\""body\""] td.offset-by-eight {\n    padding-left: 0 !important;\n  }\n  table[class=\""body\""] td.offset-by-nine {\n    padding-left: 0 !important;\n  }\n  table[class=\""body\""] td.offset-by-ten {\n    padding-left: 0 !important;\n  }\n  table[class=\""body\""] td.offset-by-eleven {\n    padding-left: 0 !important;\n  }\n  table[class=\""body\""] .expander {\n    width: 9999px !important;\n  }\n  table[class=\""body\""] .right-text-pad {\n    padding-left: 10px !important;\n  }\n  table[class=\""body\""] .text-pad-right {\n    padding-left: 10px !important;\n  }\n  table[class=\""body\""] .left-text-pad {\n    padding-right: 10px !important;\n  }\n  table[class=\""body\""] .text-pad-left {\n    padding-right: 10px !important;\n  }\n  table[class=\""body\""] .hide-for-small {\n    display: none !important;\n  }\n  table[class=\""body\""] .show-for-desktop {\n    display: none !important;\n  }\n  table[class=\""body\""] .show-for-small {\n    display: inherit !important;\n  }\n  table[class=\""body\""] .hide-for-desktop {\n    display: inherit !important;\n  }\n  table[class=\""body\""] .right-text-pad {\n    padding-left: 10px !important;\n  }\n  table[class=\""body\""] .left-text-pad {\n    padding-right: 10px !important;\n  }\n}\n</style>\n<table class=\""body\"" style=\""border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: left; height: 100%; width: 100%; color: #222222; font-family: ''Helvetica'', ''Arial'', sans-serif; font-weight: normal; line-height: 19px; font-size: 14px; margin: 0; padding: 0;\"">\n\t<tbody>\n\t\t<tr align=\""left\"" style=\""vertical-align: top; text-align: left; padding: 0;\"">\n\t\t\t<td align=\""center\"" class=\""center\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: center; padding: 0;\"" valign=\""top\"">\n\t\t\t<center style=\""width: 100%; min-width: 580px;\"">\n\t\t\t<table bgcolor=\""#5e5e5e\"" class=\""row header\"" style=\""border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: left; width: 100%; position: relative; background: #5e5e5e; padding: 0px;\"">\n\t\t\t\t<tbody>\n\t\t\t\t\t<tr align=\""left\"" style=\""vertical-align: top; text-align: left; padding: 0;\"">\n\t\t\t\t\t\t<td align=\""center\"" class=\""center\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: center; padding: 0;\"" valign=\""top\"">\n\t\t\t\t\t\t<center style=\""width: 100%; min-width: 580px;\"">\n\t\t\t\t\t\t<table class=\""container\"" style=\""border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: inherit; width: 580px; margin: 0 auto; padding: 0;\"">\n\t\t\t\t\t\t\t<tbody>\n\t\t\t\t\t\t\t\t<tr align=\""left\"" style=\""vertical-align: top; text-align: left; padding: 0;\"">\n\t\t\t\t\t\t\t\t\t<td align=\""left\"" class=\""wrapper last\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; position: relative; padding: 10px 0px 0px;\"" valign=\""top\"">\n\t\t\t\t\t\t\t\t\t<table class=\""twelve columns\"" style=\""border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: left; width: 580px; margin: 0 auto; padding: 0;\"">\n\t\t\t\t\t\t\t\t\t\t<tbody>\n\t\t\t\t\t\t\t\t\t\t\t<tr align=\""left\"" style=\""vertical-align: top; text-align: left; padding: 0;\"">\n\t\t\t\t\t\t\t\t\t\t\t\t<td align=\""left\"" class=\""six sub-columns\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; min-width: 0px; width: 50% !important; padding: 0px 10px 10px 0px;\"" valign=\""top\""><img align=\""left\"" src=\""/assets/images/email-header.jpg\"" style=\""outline: none; text-decoration: none; -ms-interpolation-mode: bicubic; width: auto; max-width: 100%; float: left; clear: both; display: block;\"" /></td>\n\t\t\t\t\t\t\t\t\t\t\t\t<td align=\""right\"" class=\""six sub-columns last\"" style=\""text-align: right; vertical-align: middle; word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; min-width: 0px; width: 50% !important; padding: 0px 0px 10px;\"" valign=\""middle\"">&nbsp;</td>\n\t\t\t\t\t\t\t\t\t\t\t\t<td align=\""left\"" class=\""expander\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; visibility: hidden; width: 0px; padding: 0;\"" valign=\""top\"">&nbsp;</td>\n\t\t\t\t\t\t\t\t\t\t\t</tr>\n\t\t\t\t\t\t\t\t\t\t</tbody>\n\t\t\t\t\t\t\t\t\t</table>\n\t\t\t\t\t\t\t\t\t</td>\n\t\t\t\t\t\t\t\t</tr>\n\t\t\t\t\t\t\t</tbody>\n\t\t\t\t\t\t</table>\n\t\t\t\t\t\t</center>\n\t\t\t\t\t\t</td>\n\t\t\t\t\t</tr>\n\t\t\t\t</tbody>\n\t\t\t</table>\n\n\t\t\t<table class=\""container\"" style=\""border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: inherit; width: 580px; margin: 0 auto; padding: 0;\"">\n\t\t\t\t<tbody>\n\t\t\t\t\t<tr align=\""left\"" style=\""vertical-align: top; text-align: left; padding: 0;\"">\n\t\t\t\t\t\t<td align=\""left\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; padding: 0;\"" valign=\""top\"">\n\t\t\t\t\t\t<table class=\""row\"" style=\""border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: left; width: 100%; position: relative; display: block; padding: 0px;\"">\n\t\t\t\t\t\t\t<tbody>\n\t\t\t\t\t\t\t\t<tr align=\""left\"" style=\""vertical-align: top; text-align: left; padding: 0;\"">\n\t\t\t\t\t\t\t\t\t<td align=\""left\"" class=\""wrapper last\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; position: relative; padding: 10px 0px 0px;\"" valign=\""top\"">\n\t\t\t\t\t\t\t\t\t<table class=\""twelve columns\"" style=\""border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: left; width: 580px; margin: 0 auto; padding: 0;\"">\n\t\t\t\t\t\t\t\t\t\t<tbody>\n\t\t\t\t\t\t\t\t\t\t\t<tr align=\""left\"" style=\""vertical-align: top; text-align: left; padding: 0;\"">\n\t\t\t\t\t\t\t\t\t\t\t\t<td align=\""left\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; padding: 0px 0px 10px;\"" valign=\""top\"">\n\t\t\t\t\t\t\t\t\t\t\t\t<p align=\""left\"" class=\""lead\"" style=\""color: #222222; font-family: ''Helvetica'', ''Arial'', sans-serif; font-weight: normal; text-align: left; line-height: 19px; font-size: 14px; margin: 0; padding: 0 0 10px;\"">&nbsp;</p>\n\n\t\t\t\t\t\t\t\t\t\t\t\t<p>{{ Person.NickName }},</p>\n\n\t\t\t\t\t\t\t\t\t\t\t\t<p>--&gt; Insert Your Communication Text Here &lt;--</p>\n\n\t\t\t\t\t\t\t\t\t\t\t\t<p>{{ Communication.ChannelData.FromName }}<br />\n\t\t\t\t\t\t\t\t\t\t\t\t<a href=\""mailto:{{ Communication.ChannelData.FromAddress }}\"" style=\""color: #2ba6cb; text-decoration: none;\"">{{ Communication.ChannelData.FromAddress }}</a></p>\n\n\t\t\t\t\t\t\t\t\t\t\t\t<p>&nbsp;</p>\n\n\t\t\t\t\t\t\t\t\t\t\t\t<table class=\""row footer\"" style=\""border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: left; width: 100%; position: relative; display: block; padding: 0px;\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t<tbody>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr align=\""left\"" style=\""vertical-align: top; text-align: left; padding: 0;\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td align=\""left\"" bgcolor=\""#ebebeb\"" class=\""wrapper\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; position: relative; background: #ebebeb; padding: 10px 20px 0px 0px;\"" valign=\""top\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\""six columns\"" style=\""border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: left; width: 280px; margin: 0 auto; padding: 0;\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tbody>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr align=\""left\"" style=\""vertical-align: top; text-align: left; padding: 0;\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td align=\""left\"" class=\""left-text-pad\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; padding: 0px 0px 10px 10px;\"" valign=\""top\""><!-- recommend social network links here --></td>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td align=\""left\"" class=\""expander\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; visibility: hidden; width: 0px; padding: 0;\"" valign=\""top\"">&nbsp;</td>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tbody>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td align=\""left\"" bgcolor=\""#ebebeb\"" class=\""wrapper last\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; position: relative; background: #ebebeb; padding: 10px 0px 0px;\"" valign=\""top\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\""six columns\"" style=\""border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: left; width: 280px; margin: 0 auto; padding: 0;\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tbody>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr align=\""left\"" style=\""vertical-align: top; text-align: left; padding: 0;\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td align=\""left\"" class=\""last right-text-pad\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; padding: 0px 0px 10px;\"" valign=\""top\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<h5 align=\""left\"" style=\""color: #222222; font-family: ''Helvetica'', ''Arial'', sans-serif; font-weight: normal; text-align: left; line-height: 1.3; word-break: normal; font-size: 24px; margin: 0; padding: 0 0 10px;\"">Contact Info:</h5>\n\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p align=\""left\"" style=\""color: #222222; font-family: ''Helvetica'', ''Arial'', sans-serif; font-weight: normal; text-align: left; line-height: 19px; font-size: 14px; margin: 0; padding: 0 0 10px;\"">{{ GlobalAttribute.OrganizationAddress }}</p>\n\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p align=\""left\"" style=\""color: #222222; font-family: ''Helvetica'', ''Arial'', sans-serif; font-weight: normal; text-align: left; line-height: 19px; font-size: 14px; margin: 12px 0 0 0; padding: 0 0 10px;\"">{{ GlobalAttribute.OrganizationPhone }}</p>\n\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p align=\""left\"" style=\""color: #222222; font-family: ''Helvetica'', ''Arial'', sans-serif; font-weight: normal; text-align: left; line-height: 19px; font-size: 14px; margin: 0; padding: 0 0 10px;\""><a href=\""mailto:{{ GlobalAttribute.OrganizationEmail }}\"" style=\""color: #2ba6cb; text-decoration: none;\"">{{ GlobalAttribute.OrganizationEmail }}</a></p>\n\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p align=\""left\"" style=\""color: #222222; font-family: ''Helvetica'', ''Arial'', sans-serif; font-weight: normal; text-align: left; line-height: 19px; font-size: 14px; margin: 0; padding: 0 0 10px;\""><a href=\""{{ GlobalAttribute.PublicApplicationRoot }}\"" style=\""color: #2ba6cb; text-decoration: none;\"">{{ GlobalAttribute.OrganizationWebsite }}</a></p>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td align=\""left\"" class=\""expander\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; visibility: hidden; width: 0px; padding: 0;\"" valign=\""top\"">&nbsp;</td>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tbody>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\n\t\t\t\t\t\t\t\t\t\t\t\t\t</tbody>\n\t\t\t\t\t\t\t\t\t\t\t\t</table>\n\n\t\t\t\t\t\t\t\t\t\t\t\t<table class=\""row\"" style=\""border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: left; width: 100%; position: relative; display: block; padding: 0px;\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t<tbody>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr align=\""left\"" style=\""vertical-align: top; text-align: left; padding: 0;\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td align=\""left\"" class=\""wrapper last\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; position: relative; padding: 10px 0px 0px;\"" valign=\""top\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\""twelve columns\"" style=\""border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: left; width: 580px; margin: 0 auto; padding: 0;\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tbody>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr align=\""left\"" style=\""vertical-align: top; text-align: left; padding: 0;\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td align=\""center\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; padding: 0px 0px 10px;\"" valign=\""top\"">\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<center style=\""width: 100%; min-width: 580px;\""><!-- recommend privacy - terms - unsubscribe here -->[[ UnsubscribeOption ]]</center>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td align=\""left\"" class=\""expander\"" style=\""word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: left; visibility: hidden; width: 0px; padding: 0;\"" valign=\""top\"">&nbsp;</td>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tbody>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\n\t\t\t\t\t\t\t\t\t\t\t\t\t</tbody>\n\t\t\t\t\t\t\t\t\t\t\t\t</table>\n\t\t\t\t\t\t\t\t\t\t\t\t<!-- container end below --></td>\n\t\t\t\t\t\t\t\t\t\t\t</tr>\n\t\t\t\t\t\t\t\t\t\t</tbody>\n\t\t\t\t\t\t\t\t\t</table>\n\t\t\t\t\t\t\t\t\t</td>\n\t\t\t\t\t\t\t\t</tr>\n\t\t\t\t\t\t\t</tbody>\n\t\t\t\t\t\t</table>\n\t\t\t\t\t\t</td>\n\t\t\t\t\t</tr>\n\t\t\t\t</tbody>\n\t\t\t</table>\n\t\t\t</center>\n\t\t\t</td>\n\t\t</tr>\n\t</tbody>\n</table>\n"" }'
WHERE [Guid] = 'AFE2ADD1-5278-441E-8E84-1DC743D99824'
AND ( [MediumDataJson] IS NULL OR [MediumDataJson] = '' )

UPDATE [CommunicationTemplate] 
SET [MediumDataJson] = N'{ ""HtmlMessage"": ""{{ GlobalAttribute.EmailStyles }}\n{{ GlobalAttribute.EmailHeader }}\n<p>{{ Person.NickName }},</p>\n\n<p>We&#39;re all about people and we&#39;d like to personalize our relationship by having a recent photo of you in our membership system. Please take a minute to send us your photo using the button below - we&#39;d really appreciate it.</p>\n\n<p><a href=\""{{ GlobalAttribute.PublicApplicationRoot }}PhotoRequest/Upload/{{ Person.UrlEncodedKey }}\"">Upload Photo </a></p>\n\n<p>Your picture will remain confidential and will only be visible to staff and volunteers in a leadership position within {{ GlobalAttribute.OrganizationName }}.</p>\n\n<p><a href=\""{{ GlobalAttribute.PublicApplicationRoot }}PhotoRequest/OptOut/{{ Person.UrlEncodedKey }}\"">I prefer not to receive future photo requests.</a></p>\n\n<p><a href=\""{{ GlobalAttribute.PublicApplicationRoot }}Unsubscribe/{{ Person.UrlEncodedKey }}\"">I&#39;m no longer involved with {{ GlobalAttribute.OrganizationName }}. Please remove me from all future communications.</a></p>\n\n<p>{{ Communication.ChannelData.FromName }}<br />\n</p>\n{{ GlobalAttribute.EmailFooter }}"" }'
WHERE [Guid] = 'B9A0489C-A823-4C5C-A9F9-14A206EC3B88'
AND ( [MediumDataJson] IS NULL OR [MediumDataJson] = '' )

-- Fix the Social Media Person Attribute Category
DECLARE @AttributeEntityTypeId int = ( SELECT TOP 1 [Id] FROM [EntityType] WHERE [Name] = 'Rock.Model.Attribute' )
UPDATE [Category] SET
	  [EntityTypeId] = @AttributeEntityTypeId
	, [EntityTypeQualifierColumn] = 'EntityTypeId'
	, [EntityTypeQualifierValue] = '15'
WHERE [Guid] = 'DD8F467D-B83C-444F-B04C-C681167046A1'
            " );
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
        }
    }
}