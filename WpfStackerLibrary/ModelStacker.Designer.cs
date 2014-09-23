﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]
#region Метаданные связи EDM

[assembly: EdmRelationshipAttribute("ModelStacker", "ProductCellContent", "Product", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(WpfStackerLibrary.Product), "CellContent", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(WpfStackerLibrary.CellContent))]

#endregion

namespace WpfStackerLibrary
{
    #region Контексты
    
    /// <summary>
    /// Нет доступной документации по метаданным.
    /// </summary>
    public partial class ModelStackerContainer : ObjectContext
    {
        #region Конструкторы
    
        /// <summary>
        /// Инициализирует новый объект ModelStackerContainer, используя строку соединения из раздела "ModelStackerContainer" файла конфигурации приложения.
        /// </summary>
        public ModelStackerContainer() : base("name=ModelStackerContainer", "ModelStackerContainer")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Инициализация нового объекта ModelStackerContainer.
        /// </summary>
        public ModelStackerContainer(string connectionString) : base(connectionString, "ModelStackerContainer")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Инициализация нового объекта ModelStackerContainer.
        /// </summary>
        public ModelStackerContainer(EntityConnection connection) : base(connection, "ModelStackerContainer")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Разделяемые методы
    
        partial void OnContextCreated();
    
        #endregion
    
        #region Свойства ObjectSet
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        public ObjectSet<Product> Products
        {
            get
            {
                if ((_Products == null))
                {
                    _Products = base.CreateObjectSet<Product>("Products");
                }
                return _Products;
            }
        }
        private ObjectSet<Product> _Products;
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        public ObjectSet<CellContent> CellContents
        {
            get
            {
                if ((_CellContents == null))
                {
                    _CellContents = base.CreateObjectSet<CellContent>("CellContents");
                }
                return _CellContents;
            }
        }
        private ObjectSet<CellContent> _CellContents;

        #endregion
        #region Методы AddTo
    
        /// <summary>
        /// Устаревший метод для добавления новых объектов в набор EntitySet Products. Взамен можно использовать метод .Add связанного свойства ObjectSet&lt;T&gt;.
        /// </summary>
        public void AddToProducts(Product product)
        {
            base.AddObject("Products", product);
        }
    
        /// <summary>
        /// Устаревший метод для добавления новых объектов в набор EntitySet CellContents. Взамен можно использовать метод .Add связанного свойства ObjectSet&lt;T&gt;.
        /// </summary>
        public void AddToCellContents(CellContent cellContent)
        {
            base.AddObject("CellContents", cellContent);
        }

        #endregion
    }
    

    #endregion
    
    #region Сущности
    
    /// <summary>
    /// Нет доступной документации по метаданным.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="ModelStacker", Name="CellContent")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class CellContent : EntityObject
    {
        #region Фабричный метод
    
        /// <summary>
        /// Создание нового объекта CellContent.
        /// </summary>
        /// <param name="id">Исходное значение свойства Id.</param>
        /// <param name="stackerID">Исходное значение свойства StackerID.</param>
        /// <param name="cellID">Исходное значение свойства CellID.</param>
        /// <param name="count">Исходное значение свойства Count.</param>
        /// <param name="changeDate">Исходное значение свойства ChangeDate.</param>
        public static CellContent CreateCellContent(global::System.Int32 id, global::System.Int32 stackerID, global::System.Int32 cellID, global::System.Int32 count, global::System.DateTime changeDate)
        {
            CellContent cellContent = new CellContent();
            cellContent.Id = id;
            cellContent.StackerID = stackerID;
            cellContent.CellID = cellID;
            cellContent.Count = count;
            cellContent.ChangeDate = changeDate;
            return cellContent;
        }

        #endregion
        #region Свойства-примитивы
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 StackerID
        {
            get
            {
                return _StackerID;
            }
            set
            {
                OnStackerIDChanging(value);
                ReportPropertyChanging("StackerID");
                _StackerID = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("StackerID");
                OnStackerIDChanged();
            }
        }
        private global::System.Int32 _StackerID;
        partial void OnStackerIDChanging(global::System.Int32 value);
        partial void OnStackerIDChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 CellID
        {
            get
            {
                return _CellID;
            }
            set
            {
                OnCellIDChanging(value);
                ReportPropertyChanging("CellID");
                _CellID = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("CellID");
                OnCellIDChanged();
            }
        }
        private global::System.Int32 _CellID;
        partial void OnCellIDChanging(global::System.Int32 value);
        partial void OnCellIDChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Count
        {
            get
            {
                return _Count;
            }
            set
            {
                OnCountChanging(value);
                ReportPropertyChanging("Count");
                _Count = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Count");
                OnCountChanged();
            }
        }
        private global::System.Int32 _Count;
        partial void OnCountChanging(global::System.Int32 value);
        partial void OnCountChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime ChangeDate
        {
            get
            {
                return _ChangeDate;
            }
            set
            {
                OnChangeDateChanging(value);
                ReportPropertyChanging("ChangeDate");
                _ChangeDate = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("ChangeDate");
                OnChangeDateChanged();
            }
        }
        private global::System.DateTime _ChangeDate;
        partial void OnChangeDateChanging(global::System.DateTime value);
        partial void OnChangeDateChanged();

        #endregion
    
        #region Свойства навигации
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ModelStacker", "ProductCellContent", "Product")]
        public Product Product
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Product>("ModelStacker.ProductCellContent", "Product").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Product>("ModelStacker.ProductCellContent", "Product").Value = value;
            }
        }
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<Product> ProductReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Product>("ModelStacker.ProductCellContent", "Product");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<Product>("ModelStacker.ProductCellContent", "Product", value);
                }
            }
        }

        #endregion
    }
    
    /// <summary>
    /// Нет доступной документации по метаданным.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="ModelStacker", Name="Product")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Product : EntityObject
    {
        #region Фабричный метод
    
        /// <summary>
        /// Создание нового объекта Product.
        /// </summary>
        /// <param name="id">Исходное значение свойства Id.</param>
        /// <param name="name">Исходное значение свойства Name.</param>
        public static Product CreateProduct(global::System.Int32 id, global::System.String name)
        {
            Product product = new Product();
            product.Id = id;
            product.Name = name;
            return product;
        }

        #endregion
        #region Свойства-примитивы
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();

        #endregion
    
        #region Свойства навигации
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ModelStacker", "ProductCellContent", "CellContent")]
        public EntityCollection<CellContent> CellContents
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<CellContent>("ModelStacker.ProductCellContent", "CellContent");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<CellContent>("ModelStacker.ProductCellContent", "CellContent", value);
                }
            }
        }

        #endregion
    }

    
#endregion    
}
