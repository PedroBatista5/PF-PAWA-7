import React from 'react';

function BtForms({ text, onClick }) {
  return (
    <button type="button" className="button-form" onClick={onClick}>
      {text}
    </button>
  );
}

export default BtForms;
