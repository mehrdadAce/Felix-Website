ALTER TABLE [dbo].[SchemeDamages] ADD [HeavyDamage] [bit]
ALTER TABLE [dbo].[SchemeDamages] ADD [Dent] [bit]
ALTER TABLE [dbo].[UserInformations] ADD [Name] [nvarchar](max) NOT NULL DEFAULT ''
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.UserInformations')
AND col_name(parent_object_id, parent_column_id) = 'Firstname';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[UserInformations] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[UserInformations] ALTER COLUMN [Firstname] [nvarchar](max) NULL
DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.UserInformations')
AND col_name(parent_object_id, parent_column_id) = 'Lastname';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[UserInformations] DROP CONSTRAINT [' + @var1 + ']')
ALTER TABLE [dbo].[UserInformations] ALTER COLUMN [Lastname] [nvarchar](max) NULL
DECLARE @var2 nvarchar(128)
SELECT @var2 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.SchemeDamages')
AND col_name(parent_object_id, parent_column_id) = 'ClientCosts';
IF @var2 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[SchemeDamages] DROP CONSTRAINT [' + @var2 + ']')
ALTER TABLE [dbo].[SchemeDamages] DROP COLUMN [ClientCosts]
DECLARE @var3 nvarchar(128)
SELECT @var3 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.SchemeDamages')
AND col_name(parent_object_id, parent_column_id) = 'Costs';
IF @var3 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[SchemeDamages] DROP CONSTRAINT [' + @var3 + ']')
ALTER TABLE [dbo].[SchemeDamages] DROP COLUMN [Costs]
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'202011241004047_Aanpassingen6', N'FelixWebsite.Dal.Migrations.Configuration',  0x1F8B0800000000000400ED5CDD6EE3B815BE2FD077307CD502B3567EBAEDEEC0D9456227B3C64E7E1067662E17B4746CB39128AF48656D147DB25EF491FA0A3D94F54352B228C98EA7E8067333A6783E9287871F0FC973F29F7FFD7BF8E33AF07B2F10711AB28BFEE9E0A4DF03E6861E658B8B7E2CE6DF7CD7FFF1873FFE6178ED05EBDEE7ACDEB9AC87928C5FF49742ACDE3B0E779710103E08A81B853C9C8B811B060EF142E7ECE4E47BE7F4D40184E82356AF377C8C99A001243FF0E728642EAC444CFCDBD0039FA7E5F8659AA0F6EE48007C455CB8E8DF804FD75F60C6A980C198F883A4E01AF10405DEEF5DFA9460AFA6E0CFFB3DC2582888C03EBFFFC4612AA2902DA62B2C20FED36605586F4E7C0EE958DE17D59B0EEBE44C0ECB2904332837E6220C5A029E9EA77A724CF14EDAEEE77A444D261ADAC85127DABCE83F2C43114ED83CECF7CCD6DE8FFC48D6ACD076A6E8412EFEAE67567A971B0ADA93FCF7AE378A7D114770C1201691ACF110CF7CEAFE0C9BA7F019D8058B7D5FED2E7618BF690558F410852B88C4E611E6E920265EBFE7E8728E29988B2932DBD14D98383FEBF7EEB07132F321B7064513531146F00118444480F740848088490C48F4596ADD68EB163C4AEC0DD683A0E946FB626CA72BE9F59C429481C96532287D6B897D197B548CC107D450067C15863E10B617D6D5C6187373D17B96898E71DE9E906B74E9A153AC87DA559210108C494016D06DA1A8086F6BE595CD7CAB6CD47DB4D14C5C2B6F89994DBE61D8F5423F0179D974921CA3B65A8ADC92E899B7ED20A17EA7FE3DE2C6A334879B2AFA0AFDDE2D597F04B6104BD99D75BF7743D7E06525A99E3F318AAE050A8928FE5DB34A62EAB8774641BABB77211603E48D5B6ADABAA111178C04F0EA66FB911CA9A1BBC334D272255D07C81BC76FF6030F8EDFE847EA02E3F0E0A3C91DBFF55104D2D4C74AE305EBB4C43A16673F6D223C65A9EA929BAF526ADBC7189E0135D9ACA85E70C22FDD5F638AC4875C71E387BF8DC26075989D63810473A0A5D6A0A551C2654769097C7FB50CD991067628DEB0D801E3714498FBFA839AF0A4ADFD0D6CC29FC833DCBF1467B2867ED8684938A79CC5C14C39CFBDD6783F05B388B8E103FA8BFBFAE8FF578E5D1C94AE549483F484DFF864515C333574F346E8EB0C00B1B97936379CBCE992A00976F5F3D0023C88FC0D5A8CAA375D6BB720ED2BDB11612E6EA2501E503E133FC69293929A358147BA581A12A7F512B2892BE23EE7F5CB13596E411338AF17487AC3716FE5B9C45FEA2524BA2EF06DBDC098F0E52C2491970BFCB55EE0E720AFF9371BB43CB2DDC782530F72A1EF9A082161A932DFD7CB5CB328F4FD000DCF983ECB8C1772DAA49C5AA6FD7A4D02CAD2D350266399FA9401EF5206CCA42CF38F3410CEEFE7CA569109DACC200E903F0C6D582CE1A7302C8CE0D46205D2F4BF2C01FC115919CD58AC2259043B442DB6F188DA282A5B8C42F6701C868612CE9A90408558032690523A1B34A183B294C5269EA29829B52D86A0CE93DE8CC51AB469D2252DA6B1B53D5DC46213792F75298B39E452C65C590CA318994EC54D2CA3A2B9738B697CA1CCE34B0A7EB1B82A367C4D24D9EB18B2F8B210B158854198E7865948A760BBFDD7B804DAC563477760BBCBA70E8102F8959D812A3EB4CCB74E875DD9B00905544B5A265CE3C2062450E6B4260450966AB02F9428ADC98E5012B22C7E9D061BEC05955C66F31076B3A0CD49A860419B8F50CD82361F61070BDA3C841D2C68731276B1A0CD5FA860419BA350C1823607C16041D34568C482CA0DD021383087FBCA0CF82184C674768BA35B34A5B0A90FEE72276D3552797671B6FF11748BF49555FD190F15E0291AB4A8FB0378B89E0476F9EF8D8F9277145AEB5CDE0A8C70D112CA8A13A61E9152D2F8270EA9D2797AF9A2776A8B3A05A15F2B2054710BA13DDD27411C4E3D88FA0C5D85A43F745BC08CA7A72ABCD21357D57D4AAEB9FCDBD0D986FCA4054367476CD0F096AC56683E4AAC505AD29B6E038546DF4CDB47CD045B0CC7E515C133796FF3964418A1BA8CAFD834F63479711A134166442EBB911794AAE976B243E3595B8629984F7285FE3301F97FC51E77864C29366480163ABDC161CA237D3262C8FBA6061195647B327A8BF824AA78F61B857E1CB05D4F8775D279408D0A911736C7C9820D5498ACAC394AE9C24F852B7D6C8EAB5F91AAA0FA976E88F2A27417E655E929B529AABC43DD857A5FBACD4E301DC3BC4CA3764A566D3C319BABA4D11AD299F0800B49A3D0F66BA95EFC7596D361968176C855A1B40FCDF1B25D4885AADE99EA50B4101C154AFBD0A25749608ED6A7A4A405796D1F6135EADA16B5189512B7A30D4A296F8E963F0CAB5079E11B6119985F87B04ADED60139CB74D3DAD39615E175984B89E8514194E2E65845CC8E0A55943647BA2BA1DCB544481FCA5588B4A83946122DA32224052DF4A105BE683AD1BE3447D482595440EDC3F1794BB99850917646ACD46165276E9DDEABC257EA50EA4259B4D55253AF051F16E12D1A1516C52DB1B6012C25AC51458C9E1DAB0851290316DF5AA2562C30B5BCC54C152F88DAC414C56D663D0F27D127392F6E83554494E86045798B95AB8799686B57FFD4C2EBD4834934E753FFF4E6811898AFE981946E65CC2A79EBF9ED8C710B334C6F44EC695CA52B926D957E0FD5F5423D793D32DD7001C14056184C7FF5473E4DDCEDAC02522B9D0317DB6BC5FED9897C0AD0B2BFFE7732B11CCE3DBF7B3AD678837E08756FA82F20FA25AFF9CB84A7C622C32E241E5A4500BF85D1F34013E1FACF31CC29A33B02CF27CC83F545FF1FBD7F1E3F0E9CCAF9B5467AB70C113352B09236F64AC0EA82B023FDAA0B5455F8DC8CEE875384CE193D6A9BA85104D279387BC288552EA31D2A0F4B5F216AE5B745721C0BAFC8BCEA02A3A7241976DDC41A2B12AF3AA0A849581DC4B584AC2E8328256775003182FED90B89DC2589FE1490F59FF759E3BF1FD2B1A769E9BC63D47FA39E2626514ACDDACB50CDF4ABBDC0EE9A00B54F9BDA0F4A4985DA0FA82ABD693FC48A94A5AA15D9256569AF792CA52575201C3D37A903803D47A913A19AF949FBD1BC998374003433CF687FC8668BA953BED07E68664E5097092DE70375D8FA2B7383F61A5B65FECFDBF1A5D69330E382CA61213B9E7FCA570275813FDB9B13ECF20C7F3C6DBB9A7F86D275786D64903530A8AA3123B4A85DEC5093D0A1AA46CB21481D038CCA57564347FDFB44C33170BA2820E45F2B62E0CA460BD0AC8EEC51665F3864B5475995D2F58420686CE43212744E5C819F5DC0C5CB8A50B76B5CC5DE84DDC762158B4BCE2198F9DA9DE3D0A96F3F89A2D2FB3CBC5F256A3BC410B09B54AE977B7615532518F5A6BC8E7741C8A591FA94D8ABA990BEE5629323DD952EFA7701A5EA1BC30A98F4489F00375904E3F76C4A5EA04BDFD0D23EC282B89BECE67137887D2274B50FC7942CF04CC0538C421E7FA20D7BC1FA87FF02EDAE18F4A64B0000 , N'6.2.0-61023')

