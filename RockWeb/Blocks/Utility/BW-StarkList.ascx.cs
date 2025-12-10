// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;
using Rock.Web.UI;
using Rock.SystemGuid;

namespace RockWeb.Blocks.Utility
{
    [DisplayName( "BW-StarkList" )]
    [Category( "Utility" )]
    [Description( "Lists active small groups with a row-click action." )]
    public partial class BW_StarkList : RockBlock, ICustomGridColumns
    {
        #region Fields

        // used for private variables

        #endregion

        #region Properties

        // used for public / protected properties

        #endregion

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            gList.GridRebind += gList_GridRebind;

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            if ( !Page.IsPostBack )
            {
                BindGrid();
            }

            base.OnLoad( e );
        }

        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {

        }

        /// <summary>
        /// Handles the GridRebind event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void gList_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }

        /// <summary>
        /// Handles the OnRowSelected event of the gList control
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e"The <see cref="EventArgs"/> instance containing the event data.></param>
        protected void gList_OnRowSelected( object sender, EventArgs e)
        {
            int groupId = (int)(e as RowEventArgs).RowKeyValue;
            Response.Redirect( string.Format( "~/DetailPage?GroupId={0}", groupId ), false );
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            using(RockContext rockContext = new RockContext())
            {
                GroupService groupService = new GroupService( rockContext );

                // sample query to display a few people
                // Use AsNoTracking() since these records won't be modified, and therefore don't need to be tracked by the EF change tracker
                var qry = groupService.Queryable()
                    .Where( g => g.GroupType.Name == "Small Group" && g.IsActive )
                    .Select( g => new
                    {
                        g.Id,
                        g.Name,
                        g.Description,
                        g.CreatedDateTime,
                        g.ModifiedDateTime,
                        g.GroupCapacity
                    } );

                // sort the query based on the column that was selected to be sorted
                var sortProperty = gList.SortProperty;
                if ( gList.AllowSorting && sortProperty != null )
                {
                    qry = qry.Sort( sortProperty );
                }
                else
                {
                    qry = qry.OrderBy( g => g.Name ).ThenBy( g => g.CreatedDateTime );
                }

                // set the datasource as a query. This allows the grid to only fetch the records that need to be shown based on the grid page and page size
                gList.SetLinqDataSource( qry );
                gList.DataBind();
            }
        }

        #endregion
    }
}