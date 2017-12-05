function UnityProgress(gameInstance, progress) {
  if (!gameInstance.Module) return;
  var perc = parseInt(100*progress, 10);
  document.getElementById('stick').style.width = (perc*5)+'px';
  document.getElementById('status').textContent = perc+'%';
  if (progress == 1) {
    document.getElementById('status').textContent = 'Please wait...';
    setTimeout(function () {
      document.getElementById('loader').style.display = 'none';
      document.getElementById('gameContainer').style.display = 'block';
    }, 1000);
  }
}
