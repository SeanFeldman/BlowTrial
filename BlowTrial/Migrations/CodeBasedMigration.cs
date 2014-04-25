﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using BlowTrial.Infrastructure.Extensions;
using System.Data.Entity;
using System.Data.Common;
using System.Data.SqlServerCe;
using BlowTrial.Domain.Providers;

namespace BlowTrial.Migrations
{
    public static class CodeBasedMigration
    {
        static int ApplySqlCeCommands(string ceConnectionString, string sqlCommand)
        {
            int returnVar = 0;
            string[] cmds = sqlCommand.Split(new string[] { "go", "Go", "GO" }, StringSplitOptions.RemoveEmptyEntries);
            using (SqlCeConnection conn = new SqlCeConnection(ceConnectionString))
            {
                conn.Open();
                foreach (string q in cmds)
                {
                    SqlCeCommand cmd = new SqlCeCommand(q, conn);
                    returnVar += cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            return returnVar;
        }
        static int MoveTrialDataToExplicitMigrations(string connectionString)
        {
            return ApplySqlCeCommands(connectionString,
                "UPDATE [__MigrationHistory] SET [MigrationId] = '201403062253529_InitialTrialData',[ContextKey] = 'BlowTrial.Migrations.TrialData.TrialDataConfiguration' WHERE ContextKey = 'BlowTrial.Migrations.TrialDataConfiguration';");
        }
        static int MoveMembershipDataToExplicitMigrations(string connectionString)
        {
            return ApplySqlCeCommands(connectionString,
                "UPDATE [__MigrationHistory] SET [MigrationId] = '201403062252244_InitialMembership',[ContextKey] = 'BlowTrial.Migrations.Membership.MembershipConfiguration' WHERE ContextKey = 'BlowTrial.Migrations.MembershipConfiguration';");
        }
        static int RepairSqlMigrationHashError(string connectionString)
        {
            return ApplySqlCeCommands(connectionString,
                "UPDATE [__MigrationHistory] " +
                "SET [MigrationId] = N'201404111106531_InitialMembership'" +
                ", [ContextKey]=N'BlowTrial.Migrations.Membership.MembershipConfiguration'" +
                ", [Model]=0x1F8B0800000000000400ED5BDB6EDB38107D5F60FF41D0E322B5E2B45DB481DD227592C2D8DC103BC5BE15B4C43844254A25A934C662BF6C1FF693F61776A89B2592BAD6E90D7D8B45F20C397386E4CC30FFFDF3EFE4F543E05BF7987112D2A93D1EEDDB16A66EE811BA9EDAB1B87DF2C27EFDEAD75F26275EF060BDCBFB3D95FD6024E553FB4E88E8D071B87B8703C447017159C8C35B3172C3C0415EE81CECEFBF74C6630703840D589635B98EA920014E7EC0CF59485D1C8918F9E7A1877D9E7D879645826A5DA000F308B9786ABFF1C34F4B46903F3A0E0344E8E88A85F7C48399D9D6914F10CC6881FD5BDB429486020998EFE10DC70BC142BA5E44F001F9CB4D84A1DF2DF239CED671B8EDDE7549FB077249CE76600EE5C65CC0DCFA018E9F663A72D4E183346D173A042D9E80B6C546AE3AD1242811B91FE2E81809645BAAB8C399CF645783AA9768E5633EDA8EDEB38A3E7B053B0E46E3D1F383F1F3D1F8E5B33D6B16FB2266784A712C98EC7615AF7CE2FE8137CBF003A6531AFB7E79B2305D68AB7C804F60E40833B1B9C6B7D912E69E6D39D5718E3AB018561A932E6D4EC5D303DBBA00E1724D05174A6A588890E1B798628604F6AE901098518981136D6AD21559A992400E66F7C06B426381799BF866C83997A0E09937D1329CF9615C2CE74D18FA18D1018827F41EFBF0ED1A512F0C0807F47EA01367CBAE46CE25133E260CBBA0D8CD10DE55117E72AF56160CB8CBA5C1C697D8F41C3D9C61BA167753FBD9FE3E6CDFA7E4017BF9A76C0A3794C0DE0FA3048B87DA790E8CE282AC11AC618895CBE3BF5B1BBF8D89D7DB1BE1986214FE6A30DDEFCF7A1BCE440FCE3F85CC6B90333E78B1034167888BB3704DE891C865C1A9819744AEB14D3917E85E92002CAEA05EC3B6043BE935F693567E47A2F4CC1FC996F765FE40B7531606F27B3AAEDAFA7E89D81ACBB985B55D1661CCDCA1BE20F186F8801CF7DD72FFF1F7B78B662719B4BF35314EA1D42E9897D3AA8179393987316F7B9E9F63CED17A180F35949FACAC9795DCF564DF90CE29078AB9094B1A987A308CA88A60089F20BAF11F51661DED8E380F5D9268AFB4E3298E509DFB09F5AC76AF48279F7B16CC1F38452260114C626AFFA669A411B5D8E5B7A8D54B4A23FAC429AD52F739A97CF018CC32059CE360056E015B836CC10FC2E07770D067AEC7336DABAB91C80B2CB4706D21D7B1B545B69A7238A729A60A55B94313798E6A68EA45BD0551D9F534B8AAAA5BC0B2B35D034959D03658DBAD8C50869D51012E19BC3A35758B2F75AC3F08D49DAD9307146B2B74E2F4C2C9395FC251E6AE6EA5D5451B9CBF60FA363DE3A4F9993C8FE3D4247226E7288A40DFA5C44EF6C55AA4599DD99345FF34479062382E37643B8AD9169260D9606CA555EE9E1E3E258C0BE93F2B2477C2991768DD74BFAE21632ECFE4BABA097372E6A3E4DFD9C8DA4457D9DF15C4AD664F61B1011C46C9BA7189064D83938C1BF211331CA3B3D08F035A7714378DAE498594016BBA749761CA8D54666C68EF836ECC93540518BBE832268E622195148EC60AC55355AA7522A2B6F1EF888BCA69D19F8F6D008FC3C93445521E9F7EF966ECD576860C3256E52CEE6FAAE6E18F63A86D52A48CB1FDDAC7E479DAA36AF6FC6B77A44A5EA30C5669F866A8945E207644A1E426D69F3AE6618F43990B8D2E173554F95A06315C5477651EFD763BC0581D401EC774F5317405B3B6577749C6A0B92CC4D8E1CB53A87A2DD71D5B0D493A3BB13AD0E4B14D1BBE8C3F0C9959733CA3ABAD137952400383A4D60AE98326960548032756C6EA394135D2D2ADAE055C6A97827345E0A50458932CD8692FA76BD14FDAC5B672B2405CBBE1020723D961B4F8E82FA4EBB1191E3D938F00F26EE788925B50499AE2B393B64A39FEDB298D3B9C7B7EE7FAF8174F54122A32676C4A45F62C2D3556A313893BAB45AFC810B4863A743BE0D01AF48F61DB72B597DE23E6DE2166A8876C517755DCFDE2EA8B29F918639228EA9660F6D91556A3BA648DB54959ED0555236C5252ED876BA89F7AC019D15E3FED57A1FC31FCE0A2CDB03BF483B61BF277ABC4B6229651B1076D7AED55B31A26A217E33F771BABBF971A8CDC7C556CB77821AC6476ADA434A71E7E98DA7F25630EADF99FEFB3617BD62583FBD9A1B56FFD3D800DCADCEBF6E10EF351B07ACCEB73AA8FC60044299DB4D61BC75AEC73498FB18F05B68EDCF41A3B43DC459EAE50190634C9AE56C79479F4A8520230500E336912E48377815F41ECA045A3578C509744C82FAF5F0F82BAEC557275059CDA728C234C2541EAD6DA456643F857E02BEA6E53426D35B713994C4163C9A0B5C66C32E40F4CAEDE06FF1A24EB10CAEF9C6CE6A7037A25B2269B6878C5DDF826208DE9E1F2B80A8106E93EBB6DD76BC18D8F06361D9E0C9804EA2F0F7A3C2C687D576092D85CF0363D3DA87D79608237D6E53B3C4AE8F826C128D2F4C2A1C3BA5A54A977A95B6FB34A3BBE9B281E3974793E519341D48E70B5D6D0F65EC2A41273DEB3FB4B891E2AC8DF677452813957597BF0D455ECBE864A7A3C1ED17396B0C396FE5108B67B4ED65B08F96F4314BB95BDB5E833A7B761BED72B33CABB2877DD732C1044D7E888C1A516B9029A5DF0B12411F50EF93174390956D89BD3CB5844B18025E360E5571E48C9A3A2497EF242A63AE7C96594443FBB58024C93C804C1257D1313DF2BE67D6AB853D740C833280B27A52D850C2BD79B02E922A41D8132F51547E71207910F60FC922ED03D1E32B71B8ECFF01AB99B3CE95C0FD26E88AADA27C704AD190A7886B11D0F3F81C35EF0F0EA7F998E61C42F370000 " +
                ", [ProductVersion]=N'6.1.0-30225' " +
                "WHERE [MigrationId]=N'201403062252244_InitialMembership' " +
                "GO " +
                "UPDATE [__MigrationHistory] " +
                "SET [MigrationId] = N'201404111113506_MembershipV2'" +
                ", [ContextKey]=N'BlowTrial.Migrations.Membership.MembershipConfiguration'" +
                ", [Model]=0x1F8B0800000000000400ED5BDB6EDB38107D5F60FF41D0E3226BC56EBBE806768BD4490A631327889D62DF0A5AA21DA212A58A546A63B15FB60FFB49FB0B3BD4CD12495DEBA417F42D16C933C399C3CBCC30FFFDF3EFF8F5D6738D071C32E2D389391C1C9B06A6B6EF10BA9998115FFFFAD27CFDEAE79FC6E78EB735DE65FD9E897E3092B28979CF79706259CCBEC71E62038FD8A1CFFC351FD8BE6721C7B746C7C7BF5BC3A18501C2042CC318DF4694130FC73FE0E7D4A7360E7884DC2BDFC12E4BBF43CB224635E6C8C32C40369E986F5CFFD33224C81D9CF91E22747013FA0FC401CD4CE3D42508345A60776D1A88529F230EFA9EDC31BCE0A14F378B003E2077B90B30F45B2397E1741E27FBEE6DA7743C1253B2F60333283B621C74EB06387C96DAC89287F7B2B499DB10AC780ED6E63B31EBD8926044647F888233C49169C8E24EA66E28BA6A4CBD442B17B3C17EF49191F739CAD9311A0C072F46C31783E7C3E191318D5C1E85784271C443D1ED265AB9C4FE03EF96FE074C273472DDA2B2A02EB4953EC02770728043BEBBC5EB740A33C734ACF2384B1E980F2B8C49A636A3FCD9C834E6205CCC29E742C10C0BEE87F82DA638441C3B3788731C528181636B2AD225598991400E0E1F80D784461CB326F1F5903326406165DE054B7FEAFA513E9D37BEEF62447B209ED307ECC2B75B441DDF230CD0BB818EAD3DBB6A39172B7C46426C8361777D785746F8C1BD4A5930E03E93061B5FECD32BB4BDC474C3EF27E6F3E363D8BE2FC8163BD9A754853B4A60EF87513C8CFAFA79068C629C6C10CCA18F978BE3BF591FBF8D88D37935C2311552F8ABC675BF3DEFEC381D3D18FBE4874E8D9CE1E8E501045D22C62FFD0DA1A73C9305A7065E1231C726E3CCD1832001785C42BD856D0976D25BECC6ADEC9E04C9993F102DEF8BFC816E17A1EF89EFC9B872EBFB250A3758E8E6577659F85168F75D0B02AFCF1A10E3BE59EE3FFEFE36AF5F24BDF6B73AC649943A04F3325AD5302F23673FE6EDCFF32BCC18DAF4E3A182F28395D5B2E2BB9EE8EBD3196540313B66490D5347FD882A0986F009A21BF749659E1108430441CFB7818B681AA81C546815D74F19F36D124B2C6CB3D2EA2B2B7F4E1DA3792926CA67CB19F407229300A80B4A4CCC5F1493D4A2E647CB1EB57C33AA451F5B8559AA0B5D781C96290E53035C616F056B11F623D182B75CB3D8E17691AE77965A5B9E8D405E60AEC4880B318FBD2FD2D9146348C53065A8D2C59D88C35B4193A383064469AB55E0CAA66E004B2F140A48C282A6C1CA16A985D26CC71270C1E165D5E473A5D0B1FAF491B7D3562B209F5B6E13AB134EC6F9028EA4BBBC7F9727AD59FC39D3F739212B490A65C923AB227B34BE424100F62E6493D22FC62249254D7F5D74CFAD78098665334D8A25D7369704D306674BAD62CB76F005091917EB6785C44E38753CA59BBAAE2BC898C9D32D5DD5851939B351E2EF74646576ADB8DE25C4BD652F60B21E9C80F1BC7181067583E3341F7251A839BBA7BE1B79B4EAFCAF1B5D917F29025674692F4397902969AC69EF82AE4DCE940568BBA832C696E421991496C20A69A5CA546B454465E33F1017A5D3A23B1F9B001E8793495EA6383EF9F2D5F8ABE90CE9E5ACD259DCDD55F5C31FC751FB4C4C1163FFB58BCBB35C4BD9EDD9D7F648A5644A11ACD4F0D55029B9401C8842F14DAC3B75F4C31E873273852EF30AAA7C2987682EAA87728F7ABBEDE1AC16208FE3BAEAC0BD8459D9ABBD246DA45E14A2EDD01E5F1F951705E87B3C3D49CB177F75EB90839ED6DB843C50B727D41D2922C2D1249CF511936AB656F44C00351C1556CBA5F7522C0DC17A2A56C4EAA8A01CCBA95E57423AB94BCEB93CB49342B8711A4E35BF1250E2ABA48B69646481C879C738F606A2C360F1D15D88C51D4EF1E0B978DB9075BB4294ACC12449E6D28CDB4AAF0CBE9E8ABFC598E3B62EFB3F79FE95509E2EC6BA0C6BC78A596D913D9678B012FB8AF441AB29AF3703F62DAD7F1FBE2D16B1E9030AC5B1A529F3EC510F55B37E72F345947C8C30890DB52638FCECC2B1D65CA2745C67ACE63AB11636AE1477C3D594851DE00C6F2E0B772BBC7E1FEB60DEE4D803AE83A63BF8376BC4A6DA9CD6B0A326BB762AC51D4A445DE5AD9F8C4EABEA73B7CAEABBAF8648F5D7D16656E5C20AD4520A6333EAE0EDC4FC2B1E7362CCFE7C9F0E3B32AE43B8039E18C7C6DF3D1827E95EB5D7B7D047C2EAA0D7E7D450B5418E54006AAC9A0E95F8EA9A9E6117736C9CDAC955798A988D1CD5A022D4A8935DAEF1497A74A8B50230500E87C225C885150C6B17E21325E2BD0909B54980DCE2FCD540ABCD7E286697C3C92D6738C05410A46AAE6D64D6849839BE64EE262354D6A45B91491798161C5AE9CC3A477EC7E4EAECF02F41B216E98283934DFF0042ADA756E444350FE06B5F36247903B8A0AE7CA041B2CFEEDBD58A76EDD3875D8B870F3A81EAFB890ECF231A5F47E824D697ED750F282ADF4FE8E0B5AF0B5A3CAD68F9B2422B52F74EA3C5BC1A4CA976A99A6FBD495BBEFEC89F6AB479045291A5548E70B962D2F4EA4367127D6EB5FD7B8F0E26C85E99B432813E1F5A79F054D51DBF84493A3C8151F3A2B0C316FEC70AB67B46367B08F11F5714DBA5BD35EF33A36B3FDBEB258DB22ED25DF70A7304113C3A0DE1528B6C0ECD36ACB138D9F50EB9117439F756D899D1EB8807118729636FE5969E7989A3A24E7EFCCEA7ACF3F83A8823AC434C01D4242209714DDF44C47572BD2F3477EA0A087106A521ABF02517A1EB669723CD7DDA1228355F7E742EB107911EC7EC9A2ED003EEA3DB1DC3977883EC5D96D8AE06697644D9ECE333823621F2588AB11F0F3F81C38EB77DF53F9FA9CC4A6A380000 " +
                ", [ProductVersion]=N'6.1.0-30225' " +
                "WHERE [MigrationId]=N'201403070429267_MembershipV2'");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="providerInvariantName"></param>
        /// <returns>whether migrations were applied</returns>
        public static bool ApplyPendingMigrations<T>(string connectionString, string providerInvariantName, bool createIfNotExists = false)
            where T : DbMigrationsConfiguration, new()
        {
            bool returnVar = false;
            if (!createIfNotExists || !CreateDatabaseIfRequired(connectionString))
            {
                returnVar = MoveFromAutoToExplicitMigrations(typeof(T), connectionString);
            }
            
            var configuration = new T();
            configuration.TargetDatabase = new DbConnectionInfo(connectionString, providerInvariantName);
            var migrator = new DbMigrator(configuration);
            if (migrator.GetPendingMigrations().Any())
            {
                migrator.Update();
                returnVar = true;
            }
            return returnVar;
        }

        private static readonly object _lock = new object();
       static bool CreateDatabaseIfRequired(string connection)
        {
            lock (_lock)
            {
                SqlCeConnectionStringBuilder builder = new SqlCeConnectionStringBuilder(connection);

                if (!System.IO.File.Exists(builder.DataSource))
                {
                    //OK, try to create the database file
                    using (var engine = new SqlCeEngine(connection))
                    {
                        engine.CreateDatabase();
                    }
                    return true;
                }
                return false;
            }
        }

        static bool MoveFromAutoToExplicitMigrations(Type T, string connectionString)
        {
            if (T == typeof(BlowTrial.Migrations.Membership.MembershipConfiguration))
            {
                return RepairSqlMigrationHashError(connectionString) != 0;
            }
            /*
            if (T == typeof(BlowTrial.Migrations.TrialData.TrialDataConfiguration))
            {
                return MoveTrialDataToExplicitMigrations(connectionString) != 0;
            }
             * */
            return false;
        }

        //namespacing changed to facilitate migrations - this returns to prior namespacing
        static string GetConfigurationKey(Type T)
        {
            if (T==typeof(BlowTrial.Migrations.Membership.MembershipConfiguration))
            {
                return "BlowTrial.Migrations.MembershipConfiguration";
            }
            if (T == typeof(BlowTrial.Migrations.TrialData.TrialDataConfiguration))
            {
                return "BlowTrial.Migrations.TrialDataConfiguration";
            }
            return T.ToString();
        }
    }
}