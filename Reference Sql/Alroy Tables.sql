--�ӿ���־��
CREATE TABLE Alroy_InterfaceLog(
	Id uniqueidentifier NULL,
	Code nvarchar(max) NULL,
	Name nvarchar(max) NULL,
	IsSuccess int null,
	Total int null,
	SuccessNum int null,
	FailNum int null,
	SkipNum int null,
	Url nvarchar(max) null,
	Memo nvarchar(max) NULL,
	ExecDate datetime NULL
)

--ɾ������
--�ؼ��ʶ�Ӧ����
--delete from Alroy_KeyWordTableName;
--insert into Alroy_KeyWordTableName select a.EntityID,a.Description,a.KeyWord,b.TableName from PowerPlat.dbo.pp_Entity a inner join PowerPlat.dbo.pp_EntityTable b on a.EntityId = b.EntityID
CREATE TABLE Alroy_KeyWordTableName(
	Id uniqueidentifier null,
	[Description] [nvarchar](100) NULL,
	[KeyWord] [nvarchar](50) NOT NULL,
	[TableName] [nvarchar](100) NULL
)

--ɾ�����̼�¼
CREATE TABLE Alroy_DeleteWorkFlowLog(
	[Id] [uniqueidentifier] NOT NULL,
	Title nvarchar(max) null,
	DeleteReason nvarchar(max) null,
	[FormId] [nvarchar](max) NULL,
	[KeyWord] [nvarchar](max) NULL,
	[KeyValue] [nvarchar](max) NULL,
	[HumanId] [nvarchar](max) NULL,
	[HumanName] [nvarchar](max) NULL,
	[ExecTime] [datetime] NULL,
	IPRecord nvarchar(max) null,
	MacAdress nvarchar(max) null
)

--select * from Alroy_DeleteWorkFlowLog order by ExecTime desc
--delete from Alroy_DeleteWorkFlowLog
