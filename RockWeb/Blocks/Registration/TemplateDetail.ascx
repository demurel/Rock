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
                <h1 class="panel-title"><i class="fa fa-cogs"></i> <asp:Literal ID="lReadOnlyTitle" runat="server" /></h1>
                
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
                                <Rock:DataTextBox ID="tbName" runat="server" SourceTypeName="Rock.Model.WorkflowType, Rock" PropertyName="Name" />
                            </div>
                            <div class="col-md-6">
                                <Rock:RockCheckBox ID="cbIsActive" runat="server" Text="Active" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <Rock:CategoryPicker ID="cpCategory" runat="server" Required="true" Label="Category" EntityTypeName="Rock.Model.WorkflowType" />
                                <Rock:GroupTypePicker ID="gtpGroupType" runat="server" Label="Group Type" />
                                <Rock:GroupRolePicker ID="rpGroupTypeRole" runat="server" Label="Group Member Role" />
                                <Rock:RockDropDownList ID="ddlGroupMemberStatus" runat="server" Label="Group Member Status" />
                                <Rock:RockCheckBox ID="cbNotifyLeaders" runat="server" Label="Notify Group Leader(s)" />
                            </div>
                            <div class="col-md-6">
                                <Rock:RockCheckBox ID="cbLoginRequired" runat="server" Label="Login Required" />
                                <Rock:RockCheckBox ID="cbMultipleRegistrants" runat="server" Label="Login Required" />
                                <Rock:NumberBox ID="nbMaxRegistrants" runat="server" Label="Maximum Registrants" />
                                <Rock:RockRadioButtonList ID="rblRegistrantsInSameFamily" runat="server" Label="Registrants in same Family" />
                                <Rock:FinancialGatewayPicker ID="fgpFinancialGateway" runat="server" Label="Financial Gateway" />
                                <Rock:CurrencyBox ID="cbMinimumInitialPayment" runat="server" Label="Minimum Initial Payment" />
                            </div>
                        </div>
                    </Rock:PanelWidget>

                    <Rock:PanelWidget ID="wpPersonFields" runat="server" Title="Fields/Attributes">
                        <div class="row">
                            <div class="col-md-6">
                                <Rock:RockRadioButtonList ID="rblRequestHomeCampus" runat="server" Label="Home Campus" />
                                <Rock:RockRadioButtonList ID="rblRequestPhone" runat="server" Label="Phone" />
                                <Rock:RockRadioButtonList ID="rblRequestEmail" runat="server" Label="Email" />
                            </div>
                            <div class="col-md-6">
                                <Rock:RockRadioButtonList ID="rblRequstBirthdate" runat="server" Label="Birthdate" />
                                <Rock:RockRadioButtonList ID="rblRequestGender" runat="server" Label="Gender" />
                                <Rock:RockRadioButtonList ID="rblRequestMaritalStatus" runat="server" Label="Marital Status" />
                            </div>
                        </div>
                    </Rock:PanelWidget>

                    <Rock:PanelWidget ID="wpFees" runat="server" Title="Fees">
                        <Rock:Grid ID="gFees" runat="server" AllowPaging="false" DisplayType="Light" RowItemText="Fees">
                            <Columns>
                                <Rock:ReorderField />
                                <Rock:RockBoundField DataField="Name" HeaderText="Fee" />
                                <Rock:RockBoundField DataField="Type" HeaderText="Type" />
                                <Rock:RockBoundField DataField="Cost" HeaderText="Cost" />
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
                                <Rock:RockBoundField DataField="Percentage" HeaderText="Percentage" />
                                <Rock:RockBoundField DataField="Amount" HeaderText="Amount" />
                                <Rock:EditField OnClick="gDiscounts_Edit" />
                                <Rock:DeleteField OnClick="gDiscounts_Delete" />
                            </Columns>
                        </Rock:Grid>
                    </Rock:PanelWidget>

                    <Rock:PanelWidget ID="wpCommunications" runat="server" Title="Communications">
                        <Rock:CodeEditor ID="ceReminderEmailTemplate" runat="server" Label="Reminder Email Template" EditorMode="Liquid" EditorTheme="Rock" EditorHeight="400" />
                        <Rock:RockCheckBox ID="cbUserDefaultConfirmation" runat="server" Label="Use Default Confirmation Email" />
                        <Rock:CodeEditor ID="ceConfirmationEmailTemplate" runat="server" Label="Confirmation Email Template" EditorMode="Liquid" EditorTheme="Rock" EditorHeight="400" />
                    </Rock:PanelWidget>

                    <Rock:PanelWidget ID="wpTerms" runat="server" Title="Terms/Text">
                        <div class="row">
                            <div class="col-md-6">
                                <Rock:RockTextBox ID="tbRegistrationTerm" runat="server" Label="Registration Term" />
                                <Rock:RockTextBox ID="tbRegistrantTerm" runat="server" Label="Registrant Term" />
                            </div>
                            <div class="col-md-6">
                                <Rock:RockTextBox ID="tbFeeTerm" runat="server" Label="Fee Term" />
                                <Rock:RockTextBox ID="tbDiscountCodeTerm" runat="server" Label="Discount Code Term" />
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

                    <p class="description"><asp:Literal ID="lWorkflowTypeDescription" runat="server"></asp:Literal></p>

                    <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                    <div class="row">
                        <div class="col-md-6">
                        </div>
                        <div class="col-md-6">
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

        <Rock:ModalDialog ID="dlgDiscount" runat="server" Title="Discount Code" OnSaveClick="dlgDiscount_SaveClick" OnCancelScript="clearActiveDialog();" ValidationGroup="Discount">
            <Content>
                <asp:HiddenField ID="hfDiscountGuid" runat="server" />
                <Rock:RockTextBox ID="tbDiscountCode" runat="server" Label="Discount Code" ValidationGroup="Discount" />
                <Rock:NumberBox ID="nbDiscountPercentage" runat="server" Label="Discount Percentage" NumberType="Integer" ValidationGroup="Discount" />
                <Rock:CurrencyBox ID="cbDiscountAmount" runat="server" Label="Discount Amount" ValidationGroup="Discount" />
            </Content>
        </Rock:ModalDialog>

        <Rock:ModalDialog ID="dlgFee" runat="server" Title="Fee" OnSaveClick="dlgFee_SaveClick" OnCancelScript="clearActiveDialog();" ValidationGroup="Fee">
            <Content>
                <asp:HiddenField ID="hfFeeGuid" runat="server" />
                <Rock:RockTextBox ID="tbFeeName" runat="server" Label="Name" ValidationGroup="Fee" />
                <Rock:RockDropDownList ID="ddlFeeType" runat="server" Label="Fee Type" ValidationGroup="Fee" />
                <Rock:CurrencyBox ID="cCost" runat="server" Label="Cost" ValidationGroup="Fee" />
                <Rock:KeyValueList ID="kvlMultipleFees" runat="server" Label="Cost Values" ValidationGroup="Fee" />
                <Rock:RockCheckBox ID="cbAllowMultiple" runat="server" Label="Allow Multiple" ValidationGroup="Fee" />
            </Content>
        </Rock:ModalDialog>

        <script>

            function toggleReadOnlyActivitiesList() {
                $('.workflow-activities-readonly-list').toggle(500);
            }

            Sys.Application.add_load(function () {
                var fixHelper = function (e, ui) {
                    ui.children().each(function () {
                        $(this).width($(this).width());
                    });
                    return ui;
                };

                $('.workflow-activity-list').sortable({
                    helper: fixHelper,
                    handle: '.workflow-activity-reorder',
                    containment: 'parent',
                    tolerance: 'pointer',
                    start: function (event, ui) {
                        {
                            var start_pos = ui.item.index();
                            ui.item.data('start_pos', start_pos);
                        }
                    },
                    update: function (event, ui) {
                        {
                            __doPostBack('<%=upDetail.ClientID %>', 're-order-activity:' + ui.item.attr('data-key') + ';' + ui.item.index());
                        }
                    }
                });

                $('.workflow-action-list').sortable({
                    helper: fixHelper,
                    handle: '.workflow-action-reorder',
                    containment: 'parent',
                    tolerance: 'pointer',
                    start: function (event, ui) {
                        {
                            var start_pos = ui.item.index();
                            ui.item.data('start_pos', start_pos);
                        }
                    },
                    update: function (event, ui) {
                        {
                            __doPostBack('<%=upDetail.ClientID %>', 're-order-action:' + ui.item.attr('data-key') + ';' + ui.item.index());
                        }
                    }
                });

                $('.workflow-formfield-list').sortable({
                    helper: fixHelper,
                    handle: '.workflow-formfield-reorder',
                    containment: 'parent',
                    tolerance: 'pointer',
                    start: function (event, ui) {
                        {
                            var start_pos = ui.item.index();
                            ui.item.data('start_pos', start_pos);
                        }
                    },
                    update: function (event, ui) {
                        {
                            __doPostBack('<%=upDetail.ClientID %>', 're-order-formfield:' + ui.item.attr('data-key') + ';' + ui.item.index());
                        }
                    }
                });

            });
        </script>
    </ContentTemplate>
</asp:UpdatePanel>
