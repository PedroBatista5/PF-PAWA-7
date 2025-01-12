import React from 'react';
import '../styles/layout.css';

function Input({ type = "text", placeholder, value, onChange }) {
  return (
    <div className="form-group">
      <input
        type={type}
        placeholder={placeholder}
        value={value}
        onChange={onChange}
        className="input-field"
      />
    </div>
  );
}

export default Input;
