function downloadFileFromBase64(base64, fileName) {
    const link = document.createElement('a');
    link.href = `data:application/pdf;base64,${base64}`;
    link.download = fileName;
    link.click();
}