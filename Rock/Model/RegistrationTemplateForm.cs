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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Newtonsoft.Json;

using Rock.Data;
using Rock.Security;

namespace Rock.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Table( "RegistrationTemplateForm" )]
    [DataContract]
    public partial class RegistrationTemplateForm : Model<RegistrationTemplateForm>
    {

        #region Entity Properties

        [Required]
        [MaxLength( 100 )]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        [DataMember]
        public int? RegistrationTemplateId { get; set; }

        #endregion

        #region Virtual Properties

        public virtual RegistrationTemplate RegistrationTemplate { get; set; }

        [DataMember]
        public virtual ICollection<RegistrationTemplateFormAttribute> FormAttributes
        {
            get { return _registrationTemplateFormAttributes ?? ( _registrationTemplateFormAttributes = new Collection<RegistrationTemplateFormAttribute>() ); }
            set { _registrationTemplateFormAttributes = value; }
        }
        private ICollection<RegistrationTemplateFormAttribute> _registrationTemplateFormAttributes;
        #endregion

        #region Methods

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion

    }

    #region Entity Configuration

    /// <summary>
    /// Configuration class.
    /// </summary>
    public partial class RegistrationTemplateFormConfiguration : EntityTypeConfiguration<RegistrationTemplateForm>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationTemplateFormConfiguration"/> class.
        /// </summary>
        public RegistrationTemplateFormConfiguration()
        {
            this.HasRequired( i => i.RegistrationTemplate ).WithMany( t => t.Forms ).HasForeignKey( i => i.RegistrationTemplateId ).WillCascadeOnDelete( true );
        }
    }

    #endregion

    #region Enumerations

    /// <summary>
    /// Represents how a <see cref="Rock.Model.Page"/> should be displayed in the page navigation controls.
    /// </summary>
    public enum RequestField
    {
        /// <summary>
        /// Do NOT display field to user
        /// </summary>
        No = 0,

        /// <summary>
        /// Display the field to user
        /// </summary>
        Yes = 1,

        /// <summary>
        /// Require user to enter the field value
        /// </summary>
        Require = 2
    }

    #endregion
}
