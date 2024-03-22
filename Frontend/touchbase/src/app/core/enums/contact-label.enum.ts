export enum ContactLabel {
  NoLabel = 0,
  Mobile = 1,
  Work = 2,
  Home = 3,
  Main = 4,
  WorkFax = 5,
  HomeFax = 6,
  Pager = 7,
  Other = 8,
}

export const ContactLabelMap: Record<ContactLabel, string> = {
  [ContactLabel.NoLabel]: "No label",
  [ContactLabel.Mobile]: "Mobile",
  [ContactLabel.Work]: "Work",
  [ContactLabel.Home]: "Home",
  [ContactLabel.Main]: "Main",
  [ContactLabel.WorkFax]: "Work fax",
  [ContactLabel.HomeFax]: "Home fax",
  [ContactLabel.Pager]: "Pager",
  [ContactLabel.Other]: "Other",
};
