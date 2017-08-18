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
        public async void CreateGetList()
        {
            var res = Attachment.Resource(Session);
            var att = CreateAttachment();
            var id = await res.Create(att);

            Assert.NotEmpty(id);

            var att_res = res.Get(id);
            var attInstance = await att_res;

            Assert.Equal(id, attInstance.Id);

            Assert.True(GetNumItems(res.List()) > 0);

            await res.Delete(id);
        }

        [Fact]
        public async void CreateAndUpload()
        {
            var res = Attachment.Resource(Session);
            var att = CreateAttachment();
            var id = await res.Create(att);

            Assert.NotEmpty(id);
            var path = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";

            try
            {
                File.WriteAllText(path, "Test data");
                await res.Upload(id, path);
            } finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            var att_res = res.Get(id);
            var attInstance = await att_res;

            Assert.Equal(id, attInstance.Id);
            Assert.True(attInstance.Size.Value > 0);

            await res.Delete(id);
        }
    }
}
