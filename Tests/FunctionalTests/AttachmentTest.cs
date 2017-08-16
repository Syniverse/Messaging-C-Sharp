using ScgApi;
using System;
using System.IO;
using Xunit;

namespace FunctionalTest
{
    public class AttachmentTest : FunctionalTestBase
    {

        Attachment CreateAttachment()
        {
            return new Attachment()
            {
                Name = "test_upload",
                Filename = "cutecat.jpg",
                Type = "image/jpeg"
            };
        }

        [Fact]
        public void CreateGetList()
        {
            var res = Attachment.Resource(Session);
            var att = CreateAttachment();
            var id = res.Create(att).Result;

            Assert.NotEmpty(id);

            var att_res = res.Get(id);
            var attInstance = att_res.Result;

            Assert.Equal(id, attInstance.Id);

            Assert.True(GetNumItems(res.List()) > 0);

            var ok = res.Delete(id);
            ok.Wait();
        }

        [Fact]
        public void CreateAndUpload()
        {
            var res = Attachment.Resource(Session);
            var att = CreateAttachment();
            var id = res.Create(att).Result;

            Assert.NotEmpty(id);
            var path = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";

            try
            {
                File.WriteAllText(path, "Test data");
                var result = res.Upload(id, path);
                result.Wait();
            } finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            var att_res = res.Get(id);
            var attInstance = att_res.Result;

            Assert.Equal(id, attInstance.Id);
            Assert.True(attInstance.Size.Value > 0);

            res.Delete(id).Wait();
        }
    }
}
