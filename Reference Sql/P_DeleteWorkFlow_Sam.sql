USE [PowerPMDB]
GO
/****** Object:  StoredProcedure [dbo].[P_DeleteWorkFlow_Sam]    Script Date: 2020/12/14 15:10:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc [dbo].[P_DeleteWorkFlow_Sam]       
@keyvalue varchar(36),  
@tablename varchar(50)     
as      
begin      
----老流程      
delete pw_PastNodes where WorkInfoID in (select WorkInfoID from pw_Infos where keyvalue = @keyvalue)      
delete pw_Opinion where WorkInfoID in (select WorkInfoID from pw_Infos where keyvalue = @keyvalue)      
delete pw_Infos where keyvalue = @keyvalue      
---新流程 

delete pwf_pastusers where WorkInfoID in (select WorkInfoID from pwf_Infos where keyvalue =@keyvalue)     
delete pwf_PastNodes where WorkInfoID in (select WorkInfoID from pwf_Infos where keyvalue = @keyvalue)      
delete pwf_Opinion where WorkInfoID in (select WorkInfoID from pwf_Infos where keyvalue = @keyvalue) 
delete pwf_inbox where WorkInfoID in (select WorkInfoID from pwf_Infos where keyvalue = @keyvalue)     
delete pwf_Infos where keyvalue = @keyvalue      
      
update pb_defaultfield set status = 0, ApprDate=null, ApprHumId=null, ApprHumName=null   
WHERE defaultfieldid = @keyvalue

delete from pb_messages where keyvalue=@keyvalue
  
  declare @sql varchar(500)  
  
  IF Exists(select 1 FROM SysObjects m join SysColumns D ON M.ID=D.ID where  m.name=@tablename and  d.name='Status')  
  begin  
   set @sql='update '+ @tablename+' set Status=0 where Id='''+ @keyvalue +''' '  
   exec(@sql)  
  end    
     
end   


