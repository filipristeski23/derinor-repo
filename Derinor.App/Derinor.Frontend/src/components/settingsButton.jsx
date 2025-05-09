import SettingsButtonLogo from "../assets/icons/SettingsButton.svg";

function SettingsButton() {
  return (
    <div className="flex gap-[1rem] cursor-pointer">
      <h3 className="text-[1rem] font-semibold">Hello, Filip</h3>
      <img src={SettingsButtonLogo} alt="Settings" />
    </div>
  );
}

export default SettingsButton;
