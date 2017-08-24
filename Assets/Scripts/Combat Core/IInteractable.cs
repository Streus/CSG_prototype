
public interface IInteractable
{
	bool interactable{ get; set; }
	bool activated{ get; set; }

	void OnInteract();
}