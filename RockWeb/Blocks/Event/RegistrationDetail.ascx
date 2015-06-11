<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RegistrationDetail.ascx.cs" Inherits="RockWeb.Blocks.Event.RegistrationDetail" %>

<script type="text/javascript">
    function clearActiveDialog() {
        $('#<%=hfActiveDialog.ClientID %>').val('');
    }
</script>

<asp:UpdatePanel ID="upnlRegistrationDetail" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlDetails" runat="server">
            <asp:HiddenField ID="hfRegistrationId" runat="server" />

            <div class="panel panel-block">

                <div class="panel-heading clearfix">
                    <h1 class="panel-title pull-left">
                        <asp:Literal ID="lGroupIconHtml" runat="server" />
                        <asp:Literal ID="lReadOnlyTitle" runat="server" />
                    </h1>

                    <div class="panel-labels">
                        <Rock:HighlightLabel ID="hlType" runat="server" LabelType="Type" />
                        <Rock:HighlightLabel ID="hlCampus" runat="server" LabelType="Campus" />
                        <Rock:HighlightLabel ID="hlCost" runat="server" LabelType="Success" />
                    </div>
                </div>
                
                <div class="panel-body">
                    <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

                    <div id="pnlEditDetails" runat="server">

                        <div class="row">
                            <div class="col-md-6">
                                <Rock:PersonPicker ID="ppPerson" runat="server" Label="Registered By" />
                                <Rock:EmailBox ID="ebConfirmationEmail" runat="server" Label="Confirmation Email" />
                                <Rock:GroupPicker ID="gpGroup" runat="server" Label="Group" />
                            </div>
                            <div class="col-md-6">
                                <Rock:RockTextBox ID="tbFirstName" runat="server" Label="Registered by First Name" />
                                  <Rock:RockTextBox ID="tbLastName" runat="server" Label="Registered by Last Name" />
                            </div>
                        </div>

                        <div class="actions">
                            <asp:LinkButton ID="btnSave" runat="server" AccessKey="s" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                            <asp:LinkButton ID="btnCancel" runat="server" AccessKey="c" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
                        </div>

                    </div>

                    <fieldset id="fieldsetViewDetails" runat="server">

                        <Rock:RockLiteral ID ="lName" runat="server" Label="Registered By" />
                        <Rock:RockLiteral ID="lConfirmationEmail" runat="server" Label="Confirmation Email" />
                        <Rock:RockLiteral ID="lGroup" runat="server" Label="Group" />

                        <div class="actions">
                            <asp:LinkButton ID="btnEdit" runat="server" AccessKey="m" Text="Edit" CssClass="btn btn-primary" OnClick="btnEdit_Click" CausesValidation="false" />
                            <Rock:ModalAlert ID="mdDeleteWarning" runat="server" />
                            <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-link" OnClick="btnDelete_Click" CausesValidation="false" />
                        </div>

                    </fieldset>
                </div>
            </div>

        </asp:Panel>

        <asp:PlaceHolder ID="phRegistrants" runat="server" />

        <asp:HiddenField ID="hfActiveDialog" runat="server" />

    </ContentTemplate>
</asp:UpdatePanel>
