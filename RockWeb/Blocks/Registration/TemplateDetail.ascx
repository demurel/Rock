<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TemplateDetail.ascx.cs" Inherits="RockWeb.Blocks.Registration.TemplateDetail" %>


<script type="text/javascript">
    function clearActiveDialog() {
        $('#<%=hfActiveDialog.ClientID %>').val('');
    }
</script>

<asp:UpdatePanel ID="upDetail" runat="server">
    <ContentTemplate>
              
        <asp:Panel ID="pnlDetails" CssClass="panel panel-block" runat="server" Visible="false">
            <asp:HiddenField ID="hfRegistrationTemplateId" runat="server" />

            <div class="panel-heading">
                <h1 class="panel-title"><i class="fa fa-clipboard"></i> <asp:Literal ID="lReadOnlyTitle" runat="server" /></h1>
                
                <div class="panel-labels">
                    <Rock:HighlightLabel ID="hlInactive" runat="server" LabelType="Danger" Text="Inactive" />
                    <Rock:HighlightLabel ID="hlType" runat="server" LabelType="Type" />
                </div>
            </div>
            <div class="panel-body container-fluid">

                <asp:ValidationSummary ID="vsDetails" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

                <div id="pnlEditDetails" runat="server">

                    <Rock:PanelWidget ID="pwDetails" runat="server" Title="Details" Expanded="true">
                        <div class="row">
                            <div class="col-md-6">
                                <Rock:DataTextBox ID="tbName" runat="server" SourceTypeName="Rock.Model.RegistrationTemplate, Rock" PropertyName="Name" />
                            </div>
                            <div class="col-md-6">
                                <Rock:RockCheckBox ID="cbIsActive" runat="server" Text="Active" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <Rock:CategoryPicker ID="cpCategory" runat="server" Required="true" Label="Category" EntityTypeName="Rock.Model.RegistrationTemplate" />
                                <Rock:GroupTypePicker ID="gtpGroupType" runat="server" Label="Group Type" AutoPostBack="true" OnSelectedIndexChanged="gtpGroupType_SelectedIndexChanged" />
                                <Rock:GroupRolePicker ID="rpGroupTypeRole" runat="server" Label="Group Member Role"
                                    Help="The group member role that new registrants should be added to group with." />
                                <Rock:RockDropDownList ID="ddlGroupMemberStatus" runat="server" Label="Group Member Status" 
                                    Help="The group member status that new registrants should be added to group with."/>
                                <Rock:RockCheckBox ID="cbNotifyLeaders" runat="server" Label="Notify Group Leader(s)" Text="Yes" 
                                    Help="Should leaders in the target group be notified when new people register for the group?" />
                            </div>
                            <div class="col-md-6">
                                <Rock:RockCheckBox ID="cbLoginRequired" runat="server" Label="Login Required" Text="Yes"
                                    Help="Is user required to be logged in when registering?" />
                                <div class="row">
                                    <div class="col-xs-6">
                                        <Rock:RockCheckBox ID="cbMultipleRegistrants" runat="server" Label="Allow Multiple Registrants"
                                            Help="Should user be allowed to register multiple registrants at the same time?"
                                            AutoPostBack="true" OnCheckedChanged="cbMultipleRegistrants_CheckedChanged" />
                                    </div>
                                    <div class="col-xs-6">
                                        <Rock:NumberBox ID="nbMaxRegistrants" runat="server" Label="Maximum Registrants"
                                            Help="The maximum number of registrants that user is allowed to register" Visible="false" />
                                    </div>
                                </div>
                                <Rock:RockRadioButtonList ID="rblRegistrantsInSameFamily" runat="server" Label="Registrants in same Family" RepeatDirection="Horizontal"
                                    Help="Typical relationship of registrants that user would register." />
                                <Rock:FinancialGatewayPicker ID="fgpFinancialGateway" runat="server" Label="Financial Gateway"
                                    Help="The financial gateway to use for processing registration payments." />
                                <Rock:CurrencyBox ID="cbMinimumInitialPayment" runat="server" Label="Minimum Initial Payment"
                                    Help="The minimum amount required per registrant." />
                            </div>
                        </div>
                    </Rock:PanelWidget>

                    <Rock:PanelWidget ID="wpPersonFields" runat="server" Title="Fields/Attributes">
                        <div class="row">
                            <div class="col-md-6">
                                <Rock:RockRadioButtonList ID="rblRequestHomeCampus" runat="server" Label="Show Home Campus" RepeatDirection="Horizontal" />
                                <Rock:RockRadioButtonList ID="rblRequestPhone" runat="server" Label="Show Phone" RepeatDirection="Horizontal" />
                                <Rock:RockRadioButtonList ID="rblRequestEmail" runat="server" Label="Show Email" RepeatDirection="Horizontal" />
                            </div>
                            <div class="col-md-6">
                                <Rock:RockRadioButtonList ID="rblRequstBirthdate" runat="server" Label="Show Birthdate" RepeatDirection="Horizontal" />
                                <Rock:RockRadioButtonList ID="rblRequestGender" runat="server" Label="Show Gender" RepeatDirection="Horizontal" />
                                <Rock:RockRadioButtonList ID="rblRequestMaritalStatus" runat="server" Label="Show Marital Status" RepeatDirection="Horizontal" />
                            </div>
                        </div>
                        <div class="grid">
                            <Rock:Grid ID="gAttributes" runat="server" AllowPaging="false" DisplayType="Light" RowItemText="Attribute">
                                <Columns>
                                    <Rock:ReorderField />
                                    <Rock:RockBoundField DataField="Type" HeaderText="Type" />
                                    <Rock:RockBoundField DataField="Name" HeaderText="Attribute" />
                                    <Rock:RockBoundField DataField="Description" HeaderText="Description" />
                                    <Rock:RockBoundField DataField="FieldType" HeaderText="Field Type" />
                                    <Rock:BoolField DataField="IsRequired" HeaderText="Required" />
                                    <Rock:EditField OnClick="gAttributes_Edit" />
                                    <Rock:DeleteField OnClick="gAttributes_Delete" />
                                </Columns>
                            </Rock:Grid>
                        </div>
                    </Rock:PanelWidget>

                    <Rock:PanelWidget ID="wpFees" runat="server" Title="Fees">
                        <Rock:Grid ID="gFees" runat="server" AllowPaging="false" DisplayType="Light" RowItemText="Fees">
                            <Columns>
                                <Rock:ReorderField />
                                <Rock:RockBoundField DataField="Name" HeaderText="Fee" />
                                <Rock:EnumField DataField="FeeType" HeaderText="Options" />
                                <Rock:RockBoundField DataField="Cost" HeaderText="Cost" />
                                <Rock:BoolField DataField="AllowMultiple" HeaderText="Allow Multiple" />
                                <Rock:BoolField DataField="DiscountApplies" HeaderText="Discount Applies" />
                                <Rock:EditField OnClick="gFees_Edit" />
                                <Rock:DeleteField OnClick="gFees_Delete" />
                            </Columns>
                        </Rock:Grid>
                    </Rock:PanelWidget>

                    <Rock:PanelWidget ID="wpDiscounts" runat="server" Title="Discounts">
                        <Rock:Grid ID="gDiscounts" runat="server" AllowPaging="false" DisplayType="Light" RowItemText="Discounts">
                            <Columns>
                                <Rock:ReorderField />
                                <Rock:RockBoundField DataField="Code" HeaderText="Code" />
                                <Rock:RockBoundField DataField="Discount" HeaderText="Discount" ItemStyle-HorizontalAlign="Right" />
                                <Rock:EditField OnClick="gDiscounts_Edit" />
                                <Rock:DeleteField OnClick="gDiscounts_Delete" />
                            </Columns>
                        </Rock:Grid>
                    </Rock:PanelWidget>

                    <Rock:PanelWidget ID="wpCommunications" runat="server" Title="Communications">
                        <Rock:CodeEditor ID="ceReminderEmailTemplate" runat="server" Label="Reminder Email Template" EditorMode="Liquid" EditorTheme="Rock" EditorHeight="300" />
                        <Rock:RockCheckBox ID="cbUserDefaultConfirmation" runat="server" Label="Use Default Confirmation Email" AutoPostBack="true" OnCheckedChanged="cbUserDefaultConfirmation_CheckedChanged" />
                        <Rock:CodeEditor ID="ceConfirmationEmailTemplate" runat="server" Label="Confirmation Email Template" EditorMode="Liquid" EditorTheme="Rock" EditorHeight="300" Visible="false" />
                    </Rock:PanelWidget>

                    <Rock:PanelWidget ID="wpTerms" runat="server" Title="Terms/Text">
                        <div class="row">
                            <div class="col-md-6">
                                <Rock:RockTextBox ID="tbRegistrationTerm" runat="server" Label="Registration Term" Placeholder="Registration" />
                                <Rock:RockTextBox ID="tbRegistrantTerm" runat="server" Label="Registrant Term" Placeholder="Registrant" />
                            </div>
                            <div class="col-md-6">
                                <Rock:RockTextBox ID="tbFeeTerm" runat="server" Label="Fee Term" Placeholder="Fee" />
                                <Rock:RockTextBox ID="tbDiscountCodeTerm" runat="server" Label="Discount Code Term" Placeholder="Discount" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <Rock:RockTextBox ID="tbSuccessTitle" runat="server" Label="Success Title" />
                                <Rock:RockTextBox ID="tbSuccessText" runat="server" Label="Success Text" TextMode="MultiLine" Rows="4" />
                            </div>
                        </div>
                    </Rock:PanelWidget>

                    <div class="actions">
                        <asp:LinkButton ID="btnSave" runat="server" AccessKey="s" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                        <asp:LinkButton ID="btnCancel" runat="server" AccessKey="c" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
                    </div>

                </div>

                <fieldset id="fieldsetViewDetails" runat="server">

                    <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                    <div class="row">
                        <div class="col-md-6">
                            <Rock:RockLiteral ID="lGroupType" runat="server" Label="Group Type" />
                        </div>
                        <div class="col-md-6">
                            <Rock:RockLiteral ID="lGateway" runat="server" Label="Gateway" />
                            <Rock:RockControlWrapper ID="rcwFees" runat="server" Label="Fees">
                                <asp:Repeater ID="rFees" runat="server">
                                    <ItemTemplate>
                                        <div class="row">
                                            <div class="col-xs-4"><%# Eval("Name") %></div>
                                            <div class="col-xs-8"><%# FormatFeeCost( Eval("CostValue").ToString() ) %></div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </Rock:RockControlWrapper>
                        </div>
                    </div>

                    <div class="actions">
                        <asp:LinkButton ID="btnEdit" runat="server" AccessKey="m" Text="Edit" CssClass="btn btn-primary" OnClick="btnEdit_Click" />
                        <Rock:ModalAlert ID="mdDeleteWarning" runat="server" />
                        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-link" OnClick="btnDelete_Click" CausesValidation="false" />
                        <span class="pull-right">
                            <asp:LinkButton ID="btnCopy" runat="server" Text="Copy" CssClass="btn btn-link" OnClick="btnCopy_Click" />
                            <Rock:SecurityButton ID="btnSecurity" runat="server" class="btn btn-sm btn-security" />
                        </span>

                    </div>
                
                </fieldset>

            </div>

        </asp:Panel>

        <asp:HiddenField ID="hfActiveDialog" runat="server" />

        <Rock:ModalDialog ID="dlgAttribute" runat="server" Title="Attribute" OnSaveClick="dlgAttribute_SaveClick" OnCancelScript="clearActiveDialog();" ValidationGroup="Attribute">
            <Content>
            </Content>
        </Rock:ModalDialog>

        <Rock:ModalDialog ID="dlgDiscount" runat="server" Title="Discount Code" OnSaveClick="dlgDiscount_SaveClick" OnCancelScript="clearActiveDialog();" ValidationGroup="Discount">
            <Content>
                <asp:HiddenField ID="hfDiscountGuid" runat="server" />
                <asp:ValidationSummary ID="ValidationSummaryDiscount" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" ValidationGroup="Discount" />
                <Rock:RockTextBox ID="tbDiscountCode" runat="server" Label="Discount Code" ValidationGroup="Discount" Required="true" />
                <Rock:RockRadioButtonList ID="rblDiscountType" runat="server" Label="Discount Type" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblDiscountType_SelectedIndexChanged">
                    <asp:ListItem Text="Percentage" Value="Percentage" />
                    <asp:ListItem Text="Amount" Value="Amount" />
                </Rock:RockRadioButtonList>
                <Rock:NumberBox ID="nbDiscountPercentage" runat="server" Label="Discount Percentage" NumberType="Integer" ValidationGroup="Discount"  />
                <Rock:CurrencyBox ID="cbDiscountAmount" runat="server" Label="Discount Amount" ValidationGroup="Discount" />
            </Content>
        </Rock:ModalDialog>

        <Rock:ModalDialog ID="dlgFee" runat="server" Title="Fee" OnSaveClick="dlgFee_SaveClick" OnCancelScript="clearActiveDialog();" ValidationGroup="Fee">
            <Content>
                <asp:HiddenField ID="hfFeeGuid" runat="server" />
                <asp:ValidationSummary ID="ValidationSummaryFee" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" ValidationGroup="Fee" />
                <Rock:RockTextBox ID="tbFeeName" runat="server" Label="Name" ValidationGroup="Fee" Required="true" />
                <Rock:RockRadioButtonList ID="rblFeeType" runat="server" Label="Options" ValidationGroup="Fee" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblFeeType_SelectedIndexChanged" />
                <Rock:CurrencyBox ID="cCost" runat="server" Label="Cost" ValidationGroup="Fee" />
                <Rock:KeyValueList ID="kvlMultipleFees" runat="server" Label="Costs" ValidationGroup="Fee" KeyPrompt="Option" ValuePrompt="Cost" />
                <Rock:RockCheckBox ID="cbAllowMultiple" runat="server" Label="Allow Multiple" ValidationGroup="Fee" Text="Yes"
                    Help="Should registrants be able to select more than one of this item?" />
                <Rock:RockCheckBox ID="cbDiscountApplies" runat="server" Label="Discount Applies" ValidationGroup="Fee" Text="Yes"
                    Help="Should discounts be applied to this fee?" />
            </Content>
        </Rock:ModalDialog>

    </ContentTemplate>
</asp:UpdatePanel>
