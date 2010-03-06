﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JinxBot.Plugins.ClanGnome
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[System.Data.Linq.Mapping.DatabaseAttribute(Name="Clans")]
	public partial class ClansDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertChannelView(ChannelView instance);
    partial void UpdateChannelView(ChannelView instance);
    partial void DeleteChannelView(ChannelView instance);
    partial void InsertClan(Clan instance);
    partial void UpdateClan(Clan instance);
    partial void DeleteClan(Clan instance);
    partial void InsertGateway(Gateway instance);
    partial void UpdateGateway(Gateway instance);
    partial void DeleteGateway(Gateway instance);
    #endregion
		
		public ClansDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ClansDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ClansDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ClansDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<ChannelView> ChannelViews
		{
			get
			{
				return this.GetTable<ChannelView>();
			}
		}
		
		public System.Data.Linq.Table<Clan> Clans
		{
			get
			{
				return this.GetTable<Clan>();
			}
		}
		
		public System.Data.Linq.Table<Gateway> Gateways
		{
			get
			{
				return this.GetTable<Gateway>();
			}
		}
	}
	
	[Table()]
	public partial class ChannelView : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private int _ClanID;
		
		private System.DateTime _When;
		
		private int _UserCount;
		
		private string _UserList;
		
		private bool _AllowedView;
		
		private EntityRef<Clan> _Clan;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnClanIDChanging(int value);
    partial void OnClanIDChanged();
    partial void OnWhenChanging(System.DateTime value);
    partial void OnWhenChanged();
    partial void OnUserCountChanging(int value);
    partial void OnUserCountChanged();
    partial void OnUserListChanging(string value);
    partial void OnUserListChanged();
    partial void OnAllowedViewChanging(bool value);
    partial void OnAllowedViewChanged();
    #endregion
		
		public ChannelView()
		{
			this._Clan = default(EntityRef<Clan>);
			OnCreated();
		}
		
		[Column(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_ClanID", DbType="Int NOT NULL")]
		public int ClanID
		{
			get
			{
				return this._ClanID;
			}
			set
			{
				if ((this._ClanID != value))
				{
					if (this._Clan.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnClanIDChanging(value);
					this.SendPropertyChanging();
					this._ClanID = value;
					this.SendPropertyChanged("ClanID");
					this.OnClanIDChanged();
				}
			}
		}
		
		[Column(Storage="_When", DbType="DateTime NOT NULL")]
		public System.DateTime When
		{
			get
			{
				return this._When;
			}
			set
			{
				if ((this._When != value))
				{
					this.OnWhenChanging(value);
					this.SendPropertyChanging();
					this._When = value;
					this.SendPropertyChanged("When");
					this.OnWhenChanged();
				}
			}
		}
		
		[Column(Storage="_UserCount", DbType="Int NOT NULL")]
		public int UserCount
		{
			get
			{
				return this._UserCount;
			}
			set
			{
				if ((this._UserCount != value))
				{
					this.OnUserCountChanging(value);
					this.SendPropertyChanging();
					this._UserCount = value;
					this.SendPropertyChanged("UserCount");
					this.OnUserCountChanged();
				}
			}
		}
		
		[Column(Storage="_UserList", DbType="NVarChar(2500) NOT NULL", CanBeNull=false)]
		public string UserList
		{
			get
			{
				return this._UserList;
			}
			set
			{
				if ((this._UserList != value))
				{
					this.OnUserListChanging(value);
					this.SendPropertyChanging();
					this._UserList = value;
					this.SendPropertyChanged("UserList");
					this.OnUserListChanged();
				}
			}
		}
		
		[Column(Storage="_AllowedView", DbType="Bit NOT NULL")]
		public bool AllowedView
		{
			get
			{
				return this._AllowedView;
			}
			set
			{
				if ((this._AllowedView != value))
				{
					this.OnAllowedViewChanging(value);
					this.SendPropertyChanging();
					this._AllowedView = value;
					this.SendPropertyChanged("AllowedView");
					this.OnAllowedViewChanged();
				}
			}
		}
		
		[Association(Name="Clan_ChannelView", Storage="_Clan", ThisKey="ClanID", OtherKey="ID", IsForeignKey=true)]
		public Clan Clan
		{
			get
			{
				return this._Clan.Entity;
			}
			set
			{
				Clan previousValue = this._Clan.Entity;
				if (((previousValue != value) 
							|| (this._Clan.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Clan.Entity = null;
						previousValue.ChannelViews.Remove(this);
					}
					this._Clan.Entity = value;
					if ((value != null))
					{
						value.ChannelViews.Add(this);
						this._ClanID = value.ID;
					}
					else
					{
						this._ClanID = default(int);
					}
					this.SendPropertyChanged("Clan");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="Clans")]
	public partial class Clan : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private string _Tag;
		
		private int _GatewayID;
		
		private string _DiscoverySource;
		
		private EntitySet<ChannelView> _ChannelViews;
		
		private EntityRef<Gateway> _Gateway;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnTagChanging(string value);
    partial void OnTagChanged();
    partial void OnGatewayIDChanging(int value);
    partial void OnGatewayIDChanged();
    partial void OnDiscoverySourceChanging(string value);
    partial void OnDiscoverySourceChanged();
    #endregion
		
		public Clan()
		{
			this._ChannelViews = new EntitySet<ChannelView>(new Action<ChannelView>(this.attach_ChannelViews), new Action<ChannelView>(this.detach_ChannelViews));
			this._Gateway = default(EntityRef<Gateway>);
			OnCreated();
		}
		
		[Column(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_Tag", DbType="NVarChar(8) NOT NULL", CanBeNull=false)]
		public string Tag
		{
			get
			{
				return this._Tag;
			}
			set
			{
				if ((this._Tag != value))
				{
					this.OnTagChanging(value);
					this.SendPropertyChanging();
					this._Tag = value;
					this.SendPropertyChanged("Tag");
					this.OnTagChanged();
				}
			}
		}
		
		[Column(Storage="_GatewayID", DbType="Int NOT NULL")]
		public int GatewayID
		{
			get
			{
				return this._GatewayID;
			}
			set
			{
				if ((this._GatewayID != value))
				{
					if (this._Gateway.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnGatewayIDChanging(value);
					this.SendPropertyChanging();
					this._GatewayID = value;
					this.SendPropertyChanged("GatewayID");
					this.OnGatewayIDChanged();
				}
			}
		}
		
		[Column(Storage="_DiscoverySource", DbType="NVarChar(1000) NOT NULL", CanBeNull=false)]
		public string DiscoverySource
		{
			get
			{
				return this._DiscoverySource;
			}
			set
			{
				if ((this._DiscoverySource != value))
				{
					this.OnDiscoverySourceChanging(value);
					this.SendPropertyChanging();
					this._DiscoverySource = value;
					this.SendPropertyChanged("DiscoverySource");
					this.OnDiscoverySourceChanged();
				}
			}
		}
		
		[Association(Name="Clan_ChannelView", Storage="_ChannelViews", ThisKey="ID", OtherKey="ClanID")]
		public EntitySet<ChannelView> ChannelViews
		{
			get
			{
				return this._ChannelViews;
			}
			set
			{
				this._ChannelViews.Assign(value);
			}
		}
		
		[Association(Name="Gateway_Clan", Storage="_Gateway", ThisKey="GatewayID", OtherKey="ID", IsForeignKey=true)]
		public Gateway Gateway
		{
			get
			{
				return this._Gateway.Entity;
			}
			set
			{
				Gateway previousValue = this._Gateway.Entity;
				if (((previousValue != value) 
							|| (this._Gateway.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Gateway.Entity = null;
						previousValue.Clans.Remove(this);
					}
					this._Gateway.Entity = value;
					if ((value != null))
					{
						value.Clans.Add(this);
						this._GatewayID = value.ID;
					}
					else
					{
						this._GatewayID = default(int);
					}
					this.SendPropertyChanged("Gateway");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_ChannelViews(ChannelView entity)
		{
			this.SendPropertyChanging();
			entity.Clan = this;
		}
		
		private void detach_ChannelViews(ChannelView entity)
		{
			this.SendPropertyChanging();
			entity.Clan = null;
		}
	}
	
	[Table(Name="Gateways")]
	public partial class Gateway : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private string _Name;
		
		private EntitySet<Clan> _Clans;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    #endregion
		
		public Gateway()
		{
			this._Clans = new EntitySet<Clan>(new Action<Clan>(this.attach_Clans), new Action<Clan>(this.detach_Clans));
			OnCreated();
		}
		
		[Column(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_Name", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[Association(Name="Gateway_Clan", Storage="_Clans", ThisKey="ID", OtherKey="GatewayID")]
		public EntitySet<Clan> Clans
		{
			get
			{
				return this._Clans;
			}
			set
			{
				this._Clans.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Clans(Clan entity)
		{
			this.SendPropertyChanging();
			entity.Gateway = this;
		}
		
		private void detach_Clans(Clan entity)
		{
			this.SendPropertyChanging();
			entity.Gateway = null;
		}
	}
}
#pragma warning restore 1591