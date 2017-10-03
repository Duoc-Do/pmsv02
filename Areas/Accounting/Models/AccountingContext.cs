using System.Data.Entity;


namespace WebApp.Areas.Accounting.Models
{
    public partial class WebAppAccEntities : DbContext
    {
        public WebAppAccEntities()
            : base("name=WebAppAccEntities")
        {
            Database.SetInitializer<WebAppAccEntities>(null);
        }

        public WebAppAccEntities(string connectionstring)
            : base(connectionstring)
        {
            Database.SetInitializer<WebAppAccEntities>(null);
        }

        #region sen việt - hệ thống
        public DbSet<AppVoucherTable> AppVoucherTables { get; set; }
        public DbSet<AppVoucherView> AppVoucherViews { get; set; }

        public DbSet<SysReport> SysReports { get; set; }
        public DbSet<SysBusiness> SysBusinesses { get; set; }
        public DbSet<SysBusinessRole> SysBusinessRoles { get; set; }
        public DbSet<SysRole> SysRoles { get; set; }
        public DbSet<SysMenu> SysMenus { get; set; }
        public DbSet<SysMenuRole> SysMenuRoles { get; set; }
        public DbSet<SysTableDetailView> SysTableDetailViews { get; set; }
        public DbSet<SysTableDetail> SysTableDetails { get; set; }

        public DbSet<SysTable> SysTables { get; set; }
        public DbSet<SysColumn> SysColumns { get; set; }
        #endregion

        #region res danh mục
        public DbSet<ResOrderItem> ResOrderItems { get; set; }
        public DbSet<ResOrderItemView> ResOrderItemViews { get; set; } 

        public DbSet<ResOrder> ResOrders { get; set; }
        public DbSet<ResOrderView> ResOrderViews { get; set; }

        public DbSet<ResTable> ResTables { get; set; }
        public DbSet<ResTableView> ResTableViews { get; set; }
        #endregion

        #region gap danh mục
        public DbSet<GapFarm> GapFarms { get; set; }
        public DbSet<GapField> GapFields { get; set; }
        public DbSet<GapRow> GapRows { get; set; }

        public DbSet<GapTree> GapTrees { get; set; }
        public DbSet<GapSeed> GapSeeds { get; set; }

        public DbSet<GapJournal> GapJournals { get; set; }

        public DbSet<GapJournalCare> GapJournalCares { get; set; }
        public DbSet<GapJournalHarvest> GapJournalHarvests { get; set; }
        #endregion

        #region nhân sự
        public DbSet<AppWorkTable> AppWorkTables { get; set; }
        #endregion

        #region sen việt - danh mục
        public DbSet<AppAccumulatedMaterialTable> AppAccumulatedMaterialTables { get; set; }
        public DbSet<AppAccumulatedMaterialView> AppAccumulatedMaterialViews { get; set; }

        public DbSet<AppMaterialBudgetTable> AppMaterialBudgetTables { get; set; }
        public DbSet<AppMaterialBudgetView> AppMaterialBudgetViews { get; set; }

        public DbSet<AppConstructionBalanceView> AppConstructionBalanceViews { get; set; }
        public DbSet<AppConstructionBalanceTable> AppConstructionBalanceTables { get; set; }

        public DbSet<AppItemOutputRateTable> AppItemOutputRateTables { get; set; }
        public DbSet<AppItemOutputRateView> AppItemOutputRateViews { get; set; }

        public DbSet<AppItemExTable> AppItemExTables { get; set; }

        public DbSet<AppExpenseBudgetView> AppExpenseBudgetViews { get; set; }
        public DbSet<AppExpenseBudgetTable> AppExpenseBudgetTables { get; set; }

        public DbSet<AppDocumentLineTable> AppDocumentLineTables { get; set; }

        public DbSet<AppDocumentLineView> AppDocumentLineViews { get; set; }

        public DbSet<AppEquipmentTable> AppEquipmentTables { get; set; }
        public DbSet<AppFixedAssetLineView> AppFixedAssetLineViews { get; set; }
        public DbSet<AppFixedAssetLineTable> AppFixedAssetLineTables { get; set; }

        public DbSet<AppFixedAssetView> AppFixedAssetViews { get; set; }
        public DbSet<AppDepartmentTable> AppDepartmentTables { get; set; }

        public DbSet<AppExObject01View> AppExObject01Views { get; set; }
        public DbSet<AppExObject02View> AppExObject02Views { get; set; }
        public DbSet<AppExObject03View> AppExObject03Views { get; set; }
        public DbSet<AppExObject04View> AppExObject04Views { get; set; }
        public DbSet<AppExObject05View> AppExObject05Views { get; set; }
        public DbSet<AppExObject06View> AppExObject06Views { get; set; }
        public DbSet<AppExObject07View> AppExObject07Views { get; set; }
        public DbSet<AppExObject08View> AppExObject08Views { get; set; }
        public DbSet<AppExObject09View> AppExObject09Views { get; set; }
        public DbSet<AppExObject10View> AppExObject10Views { get; set; }
        public DbSet<AppExObject11View> AppExObject11Views { get; set; }
        public DbSet<AppExObject12View> AppExObject12Views { get; set; }
        public DbSet<AppExObject13View> AppExObject13Views { get; set; }
        public DbSet<AppExObject14View> AppExObject14Views { get; set; }
        public DbSet<AppExObject15View> AppExObject15Views { get; set; }
        public DbSet<AppExObject16View> AppExObject16Views { get; set; }
        public DbSet<AppExObject17View> AppExObject17Views { get; set; }
        public DbSet<AppExObject18View> AppExObject18Views { get; set; }
        public DbSet<AppExObject19View> AppExObject19Views { get; set; }
        public DbSet<AppExObject20View> AppExObject20Views { get; set; }


        public DbSet<AppDocumentVATTempTable> AppDocumentVATTempTables { get; set; }
        public DbSet<AppDocumentVATTempView> AppDocumentVATTempViews { get; set; }

        public DbSet<AppDocumentVATTypeTable> AppDocumentVATTypeTables { get; set; }

        public DbSet<AppCustomerTemporaryTable> AppCustomerTemporaryTables { get; set; }
        public DbSet<AppCustomerTemporaryView> AppCustomerTemporaryViews { get; set; }

        public DbSet<AppFixedAssetTypeTable> AppFixedAssetTypeTables { get; set; }


        public DbSet<AppFixedAssetGroupTable> AppFixedAssetGroupTables { get; set; }
        public DbSet<AppExpenseTable> AppExpenseTables { get; set; }

        public DbSet<AppSalesPriceLineView> AppSalesPriceLineViews { get; set; }
        public DbSet<AppSalesPriceLineTable> AppSalesPriceLineTables { get; set; }


        public DbSet<AppEmployeeBalanceView> AppEmployeeBalanceViews { get; set; }
        public DbSet<AppEmployeeBalanceTable> AppEmployeeBalanceTables { get; set; }

        public DbSet<AppEmployeeView> AppEmployeeViews { get; set; }
        public DbSet<AppEmployeeTable> AppEmployeeTables { get; set; }

        public DbSet<AppSalesPriceGroupTable> AppSalesPriceGroupTables { get; set; }
        public DbSet<AppForwardEntryTable> AppForwardEntryTables { get; set; }
        public DbSet<AppForwardEntryView> AppForwardEntryViews { get; set; }

        public DbSet<AppConstructionView> AppConstructionViews { get; set; }
        public DbSet<AppConstructionTable> AppConstructionTables { get; set; }

        public DbSet<AppAccountBalanceTable> AppAccountBalanceTables { get; set; }
        public DbSet<AppAccountBalanceView> AppAccountBalanceViews { get; set; }


        public DbSet<AppCurrencyTable> AppCurrencyTables { get; set; }
        public DbSet<AppExchangeRateTable> AppExchangeRateTables { get; set; }

        public DbSet<AppExchangeRateView> AppExchangeRateViews { get; set; }

        public DbSet<AppCustomerBalanceView> AppCustomerBalanceViews { get; set; }
        public DbSet<AppCustomerBalanceTable> AppCustomerBalanceTables { get; set; }

        public DbSet<AppCustomerGroupTable> AppCustomerGroupTables { get; set; }
        public DbSet<AppCustomerGroupView> AppCustomerGroupViews { get; set; }

        public DbSet<AppCustomerTable> AppCustomerTables { get; set; }
        public DbSet<AppCustomerView> AppCustomerViews { get; set; }



        public DbSet<AppItemBalanceView> AppItemBalanceViews { get; set; }
        public DbSet<AppItemBalanceTable> AppItemBalanceTables { get; set; }



        public DbSet<AppItemTable> AppItemTables { get; set; }
        public DbSet<AppItemView> AppItemViews { get; set; }

        public DbSet<AppItemMethodTypeTable> AppItemMethodTypeTables { get; set; }
        public DbSet<AppItemTypeTable> AppItemTypeTables { get; set; }

        public DbSet<AppWarehouseTable> AppWarehouseTables { get; set; }
        public DbSet<AppWarehouseView> AppWarehouseViews { get; set; }
        public DbSet<AppItemGroupTable> AppItemGroupTables { get; set; }
        public DbSet<AppItemGroupView> AppItemGroupViews { get; set; }

        public DbSet<AppAccountTable> AppAccountTables { get; set; }
        public DbSet<AppAccountView> AppAccountViews { get; set; }
        public DbSet<AppAccountView2> AppAccountView2 { get; set; }


        public DbSet<AppUnitOfMeasureLinkTable> AppUnitOfMeasureLinkTables { get; set; }
        public DbSet<AppUnitOfMeasureRateTable> AppUnitOfMeasureRateTables { get; set; }
        public DbSet<AppUnitOfMeasureRateView> AppUnitOfMeasureRateViews { get; set; }

        public DbSet<AppUnitOfMeasureTable> AppUnitOfMeasureTables { get; set; }
        public DbSet<AppUnitOfMeasureView> AppUnitOfMeasureViews { get; set; }

        public DbSet<AppSalesTaxTable> AppSalesTaxTables { get; set; }
        public DbSet<AppSalesTaxView> AppSalesTaxViews { get; set; }

        public DbSet<AppPurchaseTaxTable> AppPurchaseTaxTables { get; set; }
        public DbSet<AppPurchaseTaxView> AppPurchaseTaxViews { get; set; }

        public DbSet<SysUser> SysUsers { get; set; }
        public DbSet<SysUserView> SysUserViews { get; set; }

        public DbSet<SysCompany> SysCompanies { get; set; }

        public DbSet<SysOption> SysOptions { get; set; }

        #endregion

        #region sen việt - chứng từ
        #region Chứng từ
        public DbSet<AppDocumentTable> AppDocumentTables { get; set; }
        #endregion


        #region phiếu thu
        public DbSet<AppVCA01View> AppVCA01Views { get; set; }
        public DbSet<AppVCA01VATView> AppVCA01VATViews { get; set; }
        public DbSet<AppVCA01LineView> AppVCA01LineViews { get; set; }
        #endregion

        #region phiếu thu ngân hàng

        public DbSet<AppVCA03View> AppVCA03Views { get; set; }
        public DbSet<AppVCA03VATView> AppVCA03VATViews { get; set; }
        public DbSet<AppVCA03LineView> AppVCA03LineViews { get; set; }
        #endregion

        #region phiếu chi
        public DbSet<AppVCA02View> AppVCA02Views { get; set; }
        public DbSet<AppVCA02VATView> AppVCA02VATViews { get; set; }
        public DbSet<AppVCA02LineView> AppVCA02LineViews { get; set; }
        #endregion

        #region phiếu chi ngân hàng

        public DbSet<AppVCA04View> AppVCA04Views { get; set; }
        public DbSet<AppVCA04VATView> AppVCA04VATViews { get; set; }
        public DbSet<AppVCA04LineView> AppVCA04LineViews { get; set; }
        #endregion

        #region phiếu kế toán

        public DbSet<AppVGL01View> AppVGL01Views { get; set; }
        public DbSet<AppVGL01VATView> AppVGL01VATViews { get; set; }
        public DbSet<AppVGL01LineView> AppVGL01LineViews { get; set; }
        #endregion

        #region phiếu bán hàng kiêm phiếu xuất kho

        public DbSet<AppVSO01View> AppVSO01Views { get; set; }
        public DbSet<AppVSO01VATView> AppVSO01VATViews { get; set; }
        public DbSet<AppVSO01LineView> AppVSO01LineViews { get; set; }
        #endregion

        #region phiếu nhập hàng bán trả lại

        public DbSet<AppVSO02View> AppVSO02Views { get; set; }
        public DbSet<AppVSO02VATView> AppVSO02VATViews { get; set; }
        public DbSet<AppVSO02LineView> AppVSO02LineViews { get; set; }
        #endregion

        #region phiếu giảm giá hàng bán

        public DbSet<AppVSO03View> AppVSO03Views { get; set; }
        public DbSet<AppVSO03VATView> AppVSO03VATViews { get; set; }
        public DbSet<AppVSO03LineView> AppVSO03LineViews { get; set; }
        #endregion

        #region phiếu bán hàng dịch vụ

        public DbSet<AppVSO04View> AppVSO04Views { get; set; }
        public DbSet<AppVSO04VATView> AppVSO04VATViews { get; set; }
        public DbSet<AppVSO04LineView> AppVSO04LineViews { get; set; }

        #endregion

        #region phiếu đơn đặt hàng

        public DbSet<AppVSO05View> AppVSO05Views { get; set; }
        public DbSet<AppVSO05VATView> AppVSO05VATViews { get; set; }
        public DbSet<AppVSO05LineView> AppVSO05LineViews { get; set; }
        #endregion

        #region phiếu bán lẻ dùng barcode

        public DbSet<AppVSO06View> AppVSO06Views { get; set; }
        public DbSet<AppVSO06VATView> AppVSO06VATViews { get; set; }
        public DbSet<AppVSO06LineView> AppVSO06LineViews { get; set; }
        #endregion

        #region phiếu mua hàng

        public DbSet<AppVPO01View> AppVPO01Views { get; set; }
        public DbSet<AppVPO01VATView> AppVPO01VATViews { get; set; }
        public DbSet<AppVPO01LineView> AppVPO01LineViews { get; set; }
        #endregion

        #region phiếu nhập chi phí

        public DbSet<AppVPO02View> AppVPO02Views { get; set; }
        public DbSet<AppVPO02VATView> AppVPO02VATViews { get; set; }
        public DbSet<AppVPO02LineView> AppVPO02LineViews { get; set; }
        #endregion

        #region phiếu xuất trả nhà cung cấp

        public DbSet<AppVPO03View> AppVPO03Views { get; set; }
        public DbSet<AppVPO03VATView> AppVPO03VATViews { get; set; }
        public DbSet<AppVPO03LineView> AppVPO03LineViews { get; set; }
        #endregion

        #region phiếu mua hàng dịch vụ

        public DbSet<AppVPO04View> AppVPO04Views { get; set; }
        public DbSet<AppVPO04VATView> AppVPO04VATViews { get; set; }
        public DbSet<AppVPO04LineView> AppVPO04LineViews { get; set; }
        #endregion

        #region phiếu nhập khẩu

        public DbSet<AppVPO05View> AppVPO05Views { get; set; }
        public DbSet<AppVPO05VATView> AppVPO05VATViews { get; set; }
        public DbSet<AppVPO05LineView> AppVPO05LineViews { get; set; }
        #endregion

        #region phiếu đặt mua hàng

        public DbSet<AppVPO06View> AppVPO06Views { get; set; }
        public DbSet<AppVPO06LineView> AppVPO06LineViews { get; set; }
        #endregion

        #region phiếu nhập kho

        public DbSet<AppVIN03View> AppVIN03Views { get; set; }
        public DbSet<AppVIN03LineView> AppVIN03LineViews { get; set; }
        #endregion

        #region phiếu xuất kho
        public DbSet<AppVIN04View> AppVIN04Views { get; set; }
        public DbSet<AppVIN04LineView> AppVIN04LineViews { get; set; }
        #endregion

        #region phiếu điều chuyển
        public DbSet<AppVIN05View> AppVIN05Views { get; set; }
        public DbSet<AppVIN05LineView> AppVIN05LineViews { get; set; }
        #endregion

        #endregion

        #region sen việt - báo cáo
        public DbSet<AppGLJournalView> AppGLJournalViews { get; set; }
        #endregion
    }
}