using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImportAndPreviewApi.EntityModels
{
	public class ReaderEntity
	{
		/// <summary>
		/// Gets or sets Reader Id
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ReaderId { get; set; }

		/// <summary>
		/// Gets or sets Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets Display Name
		/// </summary>
		public string DisplayName { get; set; }
	}
}