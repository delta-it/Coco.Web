import React, { useEffect } from "react";
import styled from "styled-components";
import { EditorState, Modifier } from "draft-js";
import { DefaultButton } from "./EditorButtons";
import EditorSelection from "./EditorSelection";
import { HEADING_TYPES } from "./Utils";

const Toolbar = styled.div`
  padding: ${(p) => p.theme.size.tiny};
  border-bottom: 1px solid ${(p) => p.theme.color.light};
  background-color: ${(p) => p.theme.color.lighter};
  background-image: ${(p) => p.theme.gradientColor.primary};
  border-top-left-radius: ${(p) => p.theme.borderRadius.normal};
  border-top-right-radius: ${(p) => p.theme.borderRadius.normal};
`;

const EditorButton = styled(DefaultButton)`
  color: ${(p) => p.theme.color.lighter};
  border: 0;
  margin-right: 4px;
  :hover {
    background-color: ${(p) => p.theme.color.light};
  }

  &.actived {
    background-color: ${(p) => p.theme.color.light};
  }
`;

const Divide = styled.span`
  display: inline-block;
  border-right: 1px solid ${(p) => p.theme.color.light};
  height: 20px;
  width: 0px;
  margin: 0 8px 0 4px;
  vertical-align: middle;
`;

const SelectHeading = styled(EditorSelection)`
  background-color: ${(p) => p.theme.rgbaColor.light};
  color: ${(p) => p.theme.color.lighter};
  font-weight: 600;
  border: 0;

  :hover {
    background-color: ${(p) => p.theme.color.light};
  }
`;

export default (props) => {
  const { editorState, styles } = props;

  const toggleBlockType = (e) => {
    const value = e.target.value;
    props.toggleBlockType(value);
    focusEditor();
  };

  const toggleInlineStyle = (e) => {
    const value = e.target.value;
    props.toggleInlineStyle(value);
  };

  const currentStyle = editorState.getCurrentInlineStyle();
  const selection = editorState.getSelection();
  const blockType = editorState
    .getCurrentContent()
    .getBlockForKey(selection.getStartKey())
    .getType();

  const focusEditor = () => {
    setTimeout(() => {
      props.focusEditor();
    }, 50);
  };

  function removeInlineStyles(currentState) {
    const contentState = currentState.getCurrentContent();
    var inlineStyles = styles.filter((type) => type.type === "inline");
    const contentWithoutStyles = inlineStyles.reduce(
      (state, item) =>
        Modifier.removeInlineStyle(
          state,
          currentState.getSelection(),
          item.style
        ),
      contentState
    );

    return EditorState.push(
      currentState,
      contentWithoutStyles,
      "change-inline-style"
    );
  }

  function removeBlockStyles(currentState) {
    const contentState = currentState.getCurrentContent();
    let newEditorState = currentState;
    let contentWithoutStyles = contentState;
    contentWithoutStyles = Modifier.setBlockType(
      contentWithoutStyles,
      currentState.getSelection(),
      "unstyled"
    );

    return EditorState.push(
      newEditorState,
      contentWithoutStyles,
      "change-block-type"
    );
  }

  const clearFormat = () => {
    const helpers = [];

    helpers.push(removeInlineStyles);

    helpers.push(removeBlockStyles);

    const newEditorState = helpers.reduce(
      (state, helper) => helper(state),
      editorState
    );

    props.clearFormat(newEditorState);
  };

  const onLinkModalOpen = () => {
    if (props.onLinkModalOpen) {
      props.onLinkModalOpen(true);
    }
  };

  const onImageModalOpen = () => {
    if (props.onImageModalOpen) {
      props.onImageModalOpen(true);
    }
  };

  const onRemoveLink = (e) => {
    props.onRemoveLink(e);
  };

  useEffect(() => {
    return () => {
      return clearTimeout();
    };
  });

  const items = styles.map((item) => {
    if (item.type === "divide") {
      return <Divide />;
    } else if (item.type === "image") {
      return <EditorButton icon="image" onToggle={onImageModalOpen} />;
    } else if (item.type === "link") {
      return <EditorButton icon="link" onToggle={onLinkModalOpen} />;
    } else if (item.type === "unlink") {
      return <EditorButton icon="unlink" onToggle={onRemoveLink} />;
    } else if (item.type === "eraser") {
      return <EditorButton icon="eraser" onToggle={clearFormat} />;
    } else if (item.type === "headingStyle") {
      return (
        <SelectHeading
          options={HEADING_TYPES}
          actived={blockType}
          onToggle={toggleBlockType}
          placeholder="Heading Styles"
        />
      );
    } else if (item.type && item.type === "inline") {
      return (
        <EditorButton
          key={item.style}
          actived={item.style === blockType}
          label={item.label}
          icon={item.icon}
          onToggle={toggleBlockType}
          style={item.style}
        />
      );
    }

    return (
      <EditorButton
        key={item.style}
        actived={currentStyle.has(item.style)}
        label={item.label}
        icon={item.icon}
        onToggle={toggleInlineStyle}
        style={item.style}
      />
    );
  });

  return <Toolbar>{items}</Toolbar>;
};
