public class OpenClosePopupPrompt(PopupRequest request)
	: Prompt<OpenClosePopupPrompt.Answer>(null, null)
{
	public class Answer : PromptAnswer
	{
	}

	protected override bool CanConfirm => false;
	protected override bool CanSkip => false;

	protected override void Enable()
	{
		base.Enable();

		AppController.Instance.PopupManager.PopupClosedEvent += OnPopupClosed;

		AppController.Instance.PopupManager.RequestPopup(request);
	}

	protected override void Disable()
	{
		base.Disable();

		AppController.Instance.PopupManager.PopupClosedEvent -= OnPopupClosed;
	}

	private void OnPopupClosed(PopupRequest popupRequest)
	{
		if(popupRequest == request)
		{
			Complete(new Answer()
			{
			});
		}
	}
}