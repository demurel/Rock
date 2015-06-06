﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RegistrationInstanceDetail.ascx.cs" Inherits="RockWeb.Blocks.Event.RegistrationInstanceDetail" %>

<script type="text/javascript">
    function clearActiveDialog() {
        $('#<%=hfActiveDialog.ClientID %>').val('');
    }
</script>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlDetails" runat="server">

            <asp:HiddenField ID="hfRegistrationInstanceId" runat="server" />
        
            <div class="panel panel-block">

                <div class="panel-heading">
                    <h1 class="panel-title"><i class="fa fa-file-o"></i> <asp:Literal ID="lReadOnlyTitle" runat="server" /></h1>
                    <div class="panel-labels">
                        <Rock:HighlightLabel ID="hlInactive" runat="server" LabelType="Danger" Text="Inactive" />
                        <Rock:HighlightLabel ID="hlType" runat="server" LabelType="Type" />
                    </div>
                </div>
                <div class="panel-body">

                    <asp:ValidationSummary ID="vsDetails" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

                    <div id="pnlEditDetails" runat="server">

                        <div class="row">
                            <div class="col-md-6">
                                <Rock:RockTextBox ID="tbName" runat="server" Label="Name" />
                            </div>
                            <div class="col-md-6">
                                <Rock:RockCheckBox ID="cbIsActive" runat="server" Label="Active" Text="Yes" />
                            </div>
                        </div>

                        <Rock:CodeEditor ID="ceDetails" runat="server" Label="Details" EditorMode="Html" EditorTheme="Rock" Height="200" />

                        <div class="row">
                            <div class="col-md-6">
                                <Rock:DateTimePicker ID="dtpStart" runat="server" Label="Registration Starts" />
                                <Rock:DateTimePicker ID="dtpEnd" runat="server" Label="Registration Ends" />
                                <Rock:NumberBox ID="nbMaxAttendees" runat="server" Label="Maximum Attendees" NumberType="Integer" />
                            </div>
                            <div class="col-md-6">
                                <Rock:AccountPicker ID="apAccount" runat="server" Label="Account" Required="true" />
                                <Rock:RockTextBox ID="tbContactName" runat="server" Label="Contact Name" />
                                <Rock:EmailBox ID="ebContactEmail" runat="server" Label="Contact Email" />
                            </div>
                        </div>

                        <Rock:CodeEditor ID="ceAdditionalReminderDetails" runat="server" Label="Additional Reminder Details" EditorMode="Html" EditorTheme="Rock" Height="200" />

                        <Rock:CodeEditor ID="ceAdditionalConfirmationDetails" runat="server" Label="Additional Confirmation Details" EditorMode="Html" EditorTheme="Rock" Height="200" />

                        <div class="actions">
                            <asp:LinkButton ID="btnSave" runat="server" AccessKey="s" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                            <asp:LinkButton ID="btnCancel" runat="server" AccessKey="c" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
                        </div>
                    </div>

                    <fieldset id="fieldsetViewDetails" runat="server">
                        <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                        <div class="row">
                            <div class="col-md-6">
                                <Rock:RockLiteral ID="lName" runat="server" Label="Name" />
                                <Rock:RockLiteral ID="lAccount" runat="server" Label="Account" />
                            </div>
                            <div class="col-md-6">
                                <Rock:RockLiteral ID="lMaxAttendees" runat="server" Label="Maximum Attendees" />
                            </div>
                        </div>

                        <Rock:RockLiteral ID="lDetails" runat="server" Label="Details"></Rock:RockLiteral>

                        <div class="actions">
                            <asp:LinkButton ID="btnEdit" runat="server" AccessKey="m" Text="Edit" CssClass="btn btn-primary" OnClick="btnEdit_Click" />
                            <Rock:ModalAlert ID="mdDeleteWarning" runat="server" />
                            <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-link" OnClick="btnDelete_Click" CausesValidation="false" />
                            <span class="pull-right">
                                <asp:LinkButton ID="btnPreview" runat="server" Text="Preview" CssClass="btn btn-link" OnClick="btnPreview_Click" />
                                <Rock:SecurityButton ID="btnSecurity" runat="server" class="btn btn-sm btn-security" />
                            </span>
                        </div>

                    </fieldset>

                </div>

            </div>
                    
            <asp:Panel ID="pnlTabs" runat="server" Visible="false">

                <ul class="nav nav-pills margin-b-md">
                    <li id="liRegistrations" runat="server" class="active">
                        <asp:LinkButton ID="lbRegistrations" runat="server" Text="Registrations" OnClick="lbTab_Click" />
                    </li>
                    <li id="liRegistrants" runat="server">
                        <asp:LinkButton ID="lbRegistrants" runat="server" Text="Registrants" OnClick="lbTab_Click" />
                    </li>
                    <li id="liLinkage" runat="server">
                        <asp:LinkButton ID="lbLinkage" runat="server" Text="Linkage" OnClick="lbTab_Click" />
                    </li>
                </ul>

                <asp:Panel ID="pnlRegistrations" runat="server" Visible="false">

                    <div class="grid grid-panel">
                        <Rock:GridFilter ID="fRegistrations" runat="server" OnDisplayFilterValue="fRegistrations_DisplayFilterValue">
                            <Rock:DateRangePicker ID="drpRegistrationDateRange" runat="server" Label="Date Range" />
                            <Rock:RockDropDownList ID="ddlRegistrationPaymentStatus" runat="server" Label="Payment Status" >
                                <asp:ListItem Text="" Value="" />
                                <asp:ListItem Text="Paid in Full" Value="Paid in Full" />
                                <asp:ListItem Text="Balance Owed" Value="Balance Owed" />
                            </Rock:RockDropDownList>
                            <Rock:RockTextBox ID="tbRegistrationProcessorFirstName" runat="server" Label="Processor First Name" />
                            <Rock:RockTextBox ID="tbRegistrationProcessorLastName" runat="server" Label="Processor Last Name" />
                            <Rock:RockTextBox ID="tbRegistrationRegistrantFirstName" runat="server" Label="Registrant First Name" />
                            <Rock:RockTextBox ID="tbRegistrationRegistrantLastName" runat="server" Label="Registrant Last Name" />
                        </Rock:GridFilter>
                        <Rock:Grid ID="gRegistrations" runat="server" DisplayType="Full" AllowSorting="true" OnRowSelected="gRegistrations_RowSelected" RowItemText="Registration" >
                            <Columns>
                                <Rock:DateTimeField DataField="CreatedDateTime" HeaderText="Date" SortExpression="CreatedDateTime" />
                                <Rock:RockTemplateField HeaderText="Processor">
                                    <ItemTemplate><asp:Literal ID="lProcessor" runat="server"></asp:Literal></ItemTemplate>
                                </Rock:RockTemplateField>
                                <Rock:RockTemplateField HeaderText="Registrants">
                                    <ItemTemplate><asp:Literal ID="lRegistrants" runat="server"></asp:Literal></ItemTemplate>
                                </Rock:RockTemplateField>
                                <Rock:RockTemplateField HeaderText="Cost">
                                    <ItemTemplate><asp:Literal ID="lCost" runat="server"></asp:Literal></ItemTemplate>
                                </Rock:RockTemplateField>
                                <Rock:RockTemplateField HeaderText="Paid">
                                    <ItemTemplate><asp:Label ID="lblPaid" runat="server"></asp:Label></ItemTemplate>
                                </Rock:RockTemplateField>
                                <Rock:DeleteField OnClick="gRegistrations_Delete" />
                            </Columns>
                        </Rock:Grid>
                    </div>

                </asp:Panel>

                <asp:Panel ID="pnlRegistrants" runat="server" Visible="false">

                    <div class="grid grid-panel">
                        <Rock:GridFilter ID="fRegistrants" runat="server" OnDisplayFilterValue="fRegistrants_DisplayFilterValue">
                            <Rock:DateRangePicker ID="drpRegistrantDateRange" runat="server" Label="Date Range" />
                            <Rock:RockTextBox ID="tbRegistrantFirstName" runat="server" Label="First Name" />
                            <Rock:RockTextBox ID="tbRegistrantLastName" runat="server" Label="Last Name" />
                            <asp:PlaceHolder ID="phRegistrantFormFieldFilters" runat="server" />
                        </Rock:GridFilter>
                        <Rock:Grid ID="gRegistrants" runat="server" DisplayType="Full" AllowSorting="true" OnRowSelected="gRegistrants_RowSelected" RowItemText="Registrant" >
                            <Columns>
                                <Rock:DateTimeField DataField="CreatedDateTime" HeaderText="Date" SortExpression="CreatedDateTime" />
                                <Rock:RockTemplateField HeaderText="Registrant">
                                    <ItemTemplate><asp:Literal ID="lRegistrant" runat="server"></asp:Literal></ItemTemplate>
                                </Rock:RockTemplateField>
                                <Rock:RockTemplateField HeaderText="Group">
                                    <ItemTemplate><asp:Literal ID="lGroup" runat="server"></asp:Literal></ItemTemplate>
                                </Rock:RockTemplateField>
                            </Columns>
                        </Rock:Grid>
                    </div>

                </asp:Panel>

                <asp:Panel ID="pnlLinkages" runat="server" Visible="false">
                    Linkage
                </asp:Panel>

                <asp:HiddenField ID="hfActiveDialog" runat="server" />

            </asp:Panel> 

        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
