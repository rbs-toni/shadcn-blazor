export function setCheckBoxIndeterminate(id, indeterminate, checked) {
  var item = document.getElementById(id);
  if (!!item) {
    item.isUpdating = true;

    // Need to update Checked before Indeterminate
    item.checked = checked;
    item.indeterminate = indeterminate;

    item.isUpdating = false;
  }
}
