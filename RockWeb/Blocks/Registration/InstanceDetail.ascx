<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InstanceDetail.ascx.cs" Inherits="RockWeb.Blocks.Registration.InstanceDetail" %>

<script type="text/javascript">
    function clearActiveDialog() {
        $('#<%=hfActiveDialog.ClientID %>').val('');
    }
</script>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlDetails" runat="server" CssClass="panel panel-block">

            <asp:HiddenField ID="hfRegistrationInstanceId" runat="server" />
        
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
                            <Rock:RockTextBox ID="tbContactName" runat="server" Label="Contact Name" />
                            <Rock:EmailBox ID="ebContactEmail" runat="server" Label="Contact Email" />
                            <Rock:RockTextBox ID="tbAccountCode" runat="server" Label="Account Code" />
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
                        </div>
                        <div class="col-md-6">
                            <Rock:RockLiteral ID="lMaxAttendees" runat="server" Label="Maximum Attendees" />
                        </div>
                    </div>

                    <Rock:RockLiteral ID="lDetails" runat="server" Label="Details"></Rock:RockLiteral>

                    <div class="row">
                        <div class="col-md-6">
                            <Rock:RockLiteral ID="RockLiteral1" runat="server" Label="Name" />
                        </div>
                        <div class="col-md-6">
                            <Rock:RockLiteral ID="RockLiteral2" runat="server" Label="Maximum Attendees" />
                        </div>
                    </div>

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
        
        </asp:Panel>

        <ul class="nav nav-pills margin-b-md">
            <li id="liRegistrations" runat="server" class="active">
                <a href='#<%=divRegistrations.ClientID%>' data-toggle="pill" data-active-div="Registrations">Registrations</a>
            </li>
            <li id="liRegistrants" runat="server">
                <a href='#<%=divRegistrants.ClientID%>' data-toggle="pill" data-active-div="Activities">Activities</a>
            </li>
            <li id="liLinkage" runat="server">
                <a href='#<%=divLinkage.ClientID%>' data-toggle="pill" data-active-div="Log">Log</a>
            </li>
        </ul>

        <div id="divRegistrations" runat="server"></div>
        <div id="divRegistrants" runat="server"></div>
        <div id="divLinkage" runat="server"></div>

        <asp:HiddenField ID="hfActiveDialog" runat="server" />

    </ContentTemplate>
</asp:UpdatePanel>
