using System;
namespace Client
{
    //加上随机 这tm感觉没啥用
	public class PropertyItem
	{
		public int ID { get; set; }
		private object content;
		private object rawContent;
		private bool canRandom = false;
		private int curRandomInt;
		private float curRandomFloat;
		private Type propertyType;

		// owner
		public IDynamicProperty Owner = null;


		public object Content
		{
			get 
			{
				return GetContent();
			}
			set 
			{
				if (value != GetContent())
				{
					object oldContent = GetContent();
					SetContent(value);
					if (Owner != null)
						Owner.OnChangeProperty(ID, oldContent, value);
				}
			}
		}

		public void SetValueWithoutEvent(object content)
		{
			if (content != GetContent())
			{
				object oldContent = GetContent();
				SetContent(content);
			}
		}

		public object RawContent
		{
			get { return rawContent; }
		}

		public PropertyItem (int id , object content)
		{
			propertyType = content.GetType();
			if (propertyType == typeof(System.Int32) || propertyType == typeof(System.Single))
			{
				canRandom = true;
			}

			ID = id;
			SetContent(content);
		}

		private void SetContent(object content)
		{
			rawContent = content;
			if (canRandom)
			{
				if (propertyType == typeof(System.Int32))
				{
					curRandomInt = UnityEngine.Random.Range(1, 1000);
					this.content = (int)content + curRandomInt;
				}
				else if (propertyType == typeof(System.Single))//单精度
				{
					curRandomFloat = UnityEngine.Random.Range(1.0f, 1000.0f);
					this.content = (float)content + curRandomFloat;
				}
			}
			else
			{
				this.content = content;
			}
		}

		private object GetContent()
		{
			if (canRandom)
			{
				if (propertyType == typeof(System.Int32))
				{
					int ret = (int)this.content - curRandomInt;
					if (ret != (int)rawContent)
					{
						Message message = new Message("PropertyItemDataException", this, ID);
						message.Send();
					}
					return ret;
				}
				else if (propertyType == typeof(System.Single))
				{
					float ret = (float)this.content - curRandomFloat;
                    if (ret != (float)rawContent)
					{
						Message message = new Message("PropertyItemDataException", this, ID);
						message.Send();
					}
					return ret;
				}
			}
			return this.content;
		}
	}
}

