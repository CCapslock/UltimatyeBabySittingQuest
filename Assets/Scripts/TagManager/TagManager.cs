using System.Collections.Generic;

public static class TagManager
{
	private static readonly Dictionary<TagType, string> _tags;

	static TagManager()
	{
		_tags = new Dictionary<TagType, string>
			{
				{TagType.Player, "Player"},
				{TagType.DragAble, "DragAble"},
				{TagType.Baby, "Baby"},
				{TagType.Floor, "Floor"}
			};
	}

	public static string GetTag(TagType tagType)
	{
		return _tags[tagType];
	}
}
